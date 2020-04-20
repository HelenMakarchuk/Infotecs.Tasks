namespace Infotecs.Magazine.Application.Contracts.Page
{
    /// <summary>
    /// Интерфейс страницы приложения.
    /// </summary>
    public interface IPage
    {
        /// <summary>
        /// Отображение сообщения пользователю.
        /// </summary>
        void ShowMessage(string text);

        /// <summary>
        /// Скрытие сообщения.
        /// </summary>
        void HideMessage();

        /// <summary>
        /// Загрузка исходных данных для страницы.
        /// </summary>
        void SetData();
    }
}
