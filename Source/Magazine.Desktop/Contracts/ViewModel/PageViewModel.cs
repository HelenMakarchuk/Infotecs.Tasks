using Infotecs.Magazine.Desktop.Endpoints;
using Serilog;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Infotecs.Magazine.Desktop.Contracts.ViewModel
{
    public abstract class PageViewModel<TPage> : INotifyPropertyChanged where TPage : Page
    {
        protected RabbitMqClientEndpoint _endpoint;
        protected ILogger _logger;
        protected TPage _page;

        public PageViewModel(RabbitMqClientEndpoint endpoint,
                         ILogger logger,
                         TPage page)
        {
            _endpoint = endpoint;
            _logger = logger;
            _page = page;

            NotifyUserMessages = new ObservableCollection<string>();

            NotifyUserMessages.CollectionChanged += OnNotifyUserMessagesChanged;
            _page.Loaded += OnLoaded;
        }

        /// <summary>
        /// Событие изменения свойства для последующего обновления данных в UI.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Список сообщений для уведомления пользователя приложения.
        /// </summary>
        protected ObservableCollection<string> NotifyUserMessages { get; set; }

        /// <summary>
        /// Обработчик события изменения списка сообщений для уведомления пользователя приложения.
        /// </summary>
        private void OnNotifyUserMessagesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(NotifyUserMessages));
        }

        /// <summary>
        /// Вызов события изменения свойства для последующего обновления данных в UI.
        /// </summary>
        /// <param name="propertyName">Имя свойства (получение имени используя механизм рефлексии).</param>
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Назначение исходных свойств при загрузке страницы.
        /// </summary>
        public virtual void SetData()
        {
            NotifyUserMessages.Clear();
        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs e)
        {
            //if (!_applicationViewModel.IsLoadingAllowed)
            //    return;

            SetData();
            _page.DataContext = this;
        }
    }
}
