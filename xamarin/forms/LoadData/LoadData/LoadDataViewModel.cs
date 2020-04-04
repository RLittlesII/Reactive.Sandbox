using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData;
using DynamicData.Binding;
using Services.Coffee;

namespace LoadData
{
    public class LoadDataViewModel : ViewModelBase
    {
        private readonly ICoffeeBeanDataService _dataService;
        private ReadOnlyObservableCollection<LoadDataItemViewModel> _dataSource;

        public LoadDataViewModel(ICoffeeBeanDataService dataService)
        {
            _dataService = dataService;

            _dataService
                .DataSource
                .Transform(x => new LoadDataItemViewModel())
                .Bind(out _dataSource)
                .DisposeMany()
                .Subscribe();
        }

        public ReadOnlyObservableCollection<LoadDataItemViewModel> Items => _dataSource;

        protected override async Task ExecuteInitializeData() => await _dataService.GetAll();
    }
}