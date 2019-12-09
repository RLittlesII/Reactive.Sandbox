# Image Upload

1. Forms and their images that are ready to upload would be grabbed from Akavache cache and added to a Concurrent Queue making sure of not adding duplicate sets.
2. A Service running in the app that can be turned on and off will grab the next form in the Concurrent Queue and its associated images and upload them using an API Endpoint.
3. Upon each successful form and image set that is uploaded the form and image set gets marked as uploaded in the Akavache cache.
4. If an image in a set is not uploaded completely, the upload service will have the ability to retry individual images.
5. When a Form and its respective image set are uploaded, we make 1 more call to an artbiruary API endpoint to fire off some code.
6. Nice to have - Upload throttling and max amount of simulataneous uploads.


```csharp
public class AkavacheExplorerViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> LoadCommand { get; set; }
        public ReactiveCommand<Unit, MyTestPayload> AddRandomDataCommand { get; set; }
        public IDisposable MyDisposableSubscription { get; set; }
        [Reactive] public Subject<MyTestPayload> MyObservable { get; set; }
        public AkavacheExplorerViewModel(
                                         IScheduler mainThreadScheduler = null,
                                         IScheduler taskPoolScheduler = null,
                                         IScreen hostScreen = null)
                                         : base("Akavache Explorer",
                                         mainThreadScheduler,
                                         taskPoolScheduler,
                                         hostScreen)
        {
            MyObservable = new Subject<MyTestPayload>();
            IObserver<MyTestPayload> observer = Observer.Create<MyTestPayload>(x =>
            {
                var monkey = x; //code should reach here onNext     
                Debug.WriteLine(x.Name);
            });
            MyDisposableSubscription = MyObservable.Subscribe(observer);
            LoadCommand = ReactiveCommand
                .Create<Unit, Unit>(_ =>
                {
                    return Unit.Default;
                });
            LoadCommand
                .ThrownExceptions
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x =>
                {
                    Debug.WriteLine(x.ToString());
                });
            AddRandomDataCommand = ReactiveCommand
                .CreateFromTask<Unit, MyTestPayload>(async _ =>
                {
                    RandomGenerator generator = new RandomGenerator();
                    int rand = generator.RandomNumber(5, 100);
                    var myTestPayload = new MyTestPayload();
                    myTestPayload.Id = generator.RandomNumber(0, 9999);
                    myTestPayload.Name = generator.RandomString(4, true);
                    return myTestPayload;
                });
            AddRandomDataCommand
                .ObserveOn(RxApp.MainThreadScheduler)                
                .Subscribe(x =>
                {
                    MyObservable.OnNext(x);
                });
        }
    }
    public class MyTestPayload
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class RandomGenerator
    {
        // Generate a random number between two numbers    
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        // Generate a random string with a given size    
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        // Generate a random password    
        public string RandomPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }
    }
```

### Max Concurrent Queue

- Concurrent Queue
    - Enqueue: Puts the item on the concurrent queue
    - Dequeued: An observable of items removed from the concurrent queue
    