using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Forms
{
    public class MaxConcurrentQueue<T>
    {
        private IObservable<T> _firstRequest;
        private IObservable<T> _secondRequest;
        private IObservable<T> _thirdRequest;
        private ConcurrentQueue<T> _operationQueue = new ConcurrentQueue<T>();

        public bool FirstToken { get; set; }
        public bool SecondToken { get; set; }

        public MaxConcurrentQueue()
        {          
            _subject
            .Subscribe(queue =>
            {
                // Processes our Func to return a result

                // At the end of Func call Dequeue
                Dequeue();
            });            
        }

        private ConcurrentQueue<Unit> _inFlightQueue = new ConcurrentQueue<IObservable<T>>(); // TODO: Maybe <T> maybe IDisposable?
        private Subject<Func<T>> _subject = new Subject<Func<T>>();

        public void Enqueue(Func<T> thing)
        {
            Dequeue();
            _operationQueue.Enqueue(default);

            _subject.OnNext(thing);
            //OnNext will call Dequeue somewhere too?
        }

        public IObservable<List<T>> Dequeue()
        {
            if (!FirstToken || !SecondToken)
            {
                if (!FirstToken)
                    FirstToken = true;

                if (!SecondToken)
                    SecondToken = true;

                _operationQueue.TryDequeue(out var dequeued); //Tries to dequeue from the _operationQueue
                _subject
                    .Subscribe(queue =>
                    {
                    //Processes our Func to return a result

                    //Set whichever token it had in hand to false or being free

                    //At the end of Func call Dequeue
                });
            }
            Dequeue();
        }
        
        public IObservable<T> Dequeued { get; }
    }
}