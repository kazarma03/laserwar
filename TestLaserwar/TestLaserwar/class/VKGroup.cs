using System.ComponentModel;

namespace TestLaserwar
{
        class VKGroup : INotifyPropertyChanged
{
    /// <summary>
    /// Конструктор для упрощения создания списков групп пользователя
    /// </summary>
    /// <param name="ID">Идентификатор группы</param>
    /// <param name="Name">Имя группы</param>
    public VKGroup(long ID, string Name)
    {
        this.ID = ID;
        this.Name = Name;
    }

    //Идентификатор группы
    public long _ID;
    /// <summary>
    /// Идентификатор группы
    /// </summary>
    public long ID
    {
        get
        {
            return _ID;
        }
        set
        {
            if (_ID != value)
            {
                _ID = value;
                OnPropertyChanged("ID");
            }
        }
    }

    //Имя группы
    public string _Name;
    /// <summary>
    /// Имя группы
    /// </summary>
    public string Name
    {
        get
        {
            return _Name;
        }
        set
        {
            if (_Name != value)
            {
                _Name = value;
                OnPropertyChanged("Name");
            }
        }
    }


    public event PropertyChangedEventHandler PropertyChanged;

    // уведомления представления об изменениях свойств объекта
    protected void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
}
