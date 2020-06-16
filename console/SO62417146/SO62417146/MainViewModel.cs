using System;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

namespace SO62417146
{
    public class MainViewModel : ReactiveObject
    {
        private string _carModelName;
        private readonly ObservableAsPropertyHelper<ICarModel> _carViewModel;

        public MainViewModel()
        {
            this.WhenAnyValue(x => x.CarModelName)
                .Select<string, ICarModel>(x =>
                {
                    switch (x)
                    {
                        case "VW":
                            return new VWModel();
                        case "BMW":
                            return new BMWViewModel();
                        default:
                            return null;
                    }
                })
                .ToProperty(this, x => x.CarViewModel, out _carViewModel);

            this.WhenAnyValue(x => x.CarModelName)
                .Select<string, ICarModel>(x =>
                {
                    switch (x)
                    {
                        case "VW":
                            return new VWModel();
                        case "BMW":
                            return new BMWViewModel();
                        default:
                            return null;
                    }
                })
                .InvokeCommand(this, x => x.CarViewModel.Thing);
            this.CarModelName = "VW";
        }

        public string CarModelName
        {
            get => _carModelName;
            set => this.RaiseAndSetIfChanged(ref _carModelName, value);
        }

        public ICarModel CarViewModel => _carViewModel.Value;

        private void IssueToSolve()
        {
            this.CarModelName = "BMW";

            // now I want to call CarViewModel, but I have null, because modification of property is in progress
            // this.CarViewModel as ICarModel;
            // this.CarViewModel.Method(new Foo());

            // how to wait for CarViewModel update to call Method with parameters?
        }
    }

    public class BMWViewModel : ICarModel
    {
        public ReactiveCommand<Unit, Unit> Thing { get; set; }
    }

    public class VWModel : ICarModel
    {
        public ReactiveCommand<Unit, Unit> Thing { get; set; }
    }

    public interface ICarModel
    {
        ReactiveCommand<Unit, Unit> Thing { get; set; }
    }
}