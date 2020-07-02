using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Rocket.Surgery.Airframe.Synthetic;

namespace DynamicList.Crud
{
    public class ItemViewModel : ItemViewModelBase
    {
        private Guid _id;
        private string _title;
        private string _description;
        private DrinkType _type;

        public Guid Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }

        public string Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value);
        }

        public string Description
        {
            get => _description;
            set => this.RaiseAndSetIfChanged(ref _description, value);
        }

        public DrinkType Type
        {
            get => _type;
            set => this.RaiseAndSetIfChanged(ref _type, value);
        }
    }
}