using System.ComponentModel;
using System.Windows;

namespace TestLaserwar
{
    class Interface : INotifyPropertyChanged
    {
        //Фрма загрузки JSON
        public Visibility _dkey;
        /// <summary>
        /// Фрма загрузки JSON
        /// </summary>
        public Visibility dkey
        {
            get
            {
                return _dkey;
            }
            set
            {
                if (_dkey != value)
                {
                    _dkey = value;
                    OnPropertyChanged("dkey");
                }
            }
        }

        //орма звука
        public Visibility _skey;
        /// <summary>
        /// орма звука
        /// </summary>
        public Visibility skey
        {
            get
            {
                return _skey;
            }
            set
            {
                if (_skey != value)
                {
                    _skey = value;
                    OnPropertyChanged("skey");
                }
            }
        }

        //Форма игр
        public Visibility _gkey;
        /// <summary>
        /// Форма игр
        /// </summary>
        public Visibility gkey
        {
            get
            {
                return _gkey;
            }
            set
            {
                if (_gkey != value)
                {
                    _gkey = value;
                    OnPropertyChanged("gkey");
                }
            }
        }

        //Форма детализации игр
        public Visibility _gdkey;
        /// <summary>
        /// Форма детализации игр
        /// </summary>
        public Visibility gdkey
        {
            get
            {
                return _gdkey;
            }
            set
            {
                if (_gdkey != value)
                {
                    _gdkey = value;
                    OnPropertyChanged("gdkey");
                }
            }
        }

        //Форма детализации параметров игрока
        public Visibility _gdgkey;
        /// <summary>
        /// Форма детализации параметров игрока
        /// </summary>
        public Visibility gdgkey
        {
            get
            {
                return _gdgkey;
            }
            set
            {
                if (_gdgkey != value)
                {
                    _gdgkey = value;
                    OnPropertyChanged("gdgkey");
                }
            }
        }

        //Форма ВК
        public Visibility _VKkey;
        /// <summary>
        /// Форма ВК
        /// </summary>
        public Visibility VKkey
        {
            get
            {
                return _VKkey;
            }
            set
            {
                if (_VKkey != value)
                {
                    _VKkey = value;
                    OnPropertyChanged("VKkey");
                }
            }
        }

        //Форма Аутентификации ВК на форме ВК
        public Visibility _VKAkey;
        /// <summary>
        /// Форма Аутентификации ВК на форме ВК
        /// </summary>
        public Visibility VKAkey
        {
            get
            {
                return _VKAkey;
            }
            set
            {
                if (_VKAkey != value)
                {
                    _VKAkey = value;
                    OnPropertyChanged("VKAkey");
                }
            }
        }

        //Форма отправки Поста в ВК на формме В
        public Visibility _VKSkey;
        /// <summary>
        /// Форма отправки Поста в ВК на формме В
        /// </summary>
        public Visibility VKSkey
        {
            get
            {
                return _VKSkey;
            }
            set
            {
                if (_VKSkey != value)
                {
                    _VKSkey = value;
                    OnPropertyChanged("VKSkey");
                }
            }
        }


        //Затемнение главных форм
        public Visibility _Skey;
        /// <summary>
        /// Затемнение главных форм
        /// </summary>
        public Visibility Skey
        {
            get
            {
                return _Skey;
            }
            set
            {
                if (_Skey != value)
                {
                    _Skey = value;
                    OnPropertyChanged("Skey");
                }
            }
        }

        //Затемнение вспомогательных форм
        public Visibility _SSkey;
        /// <summary>
        /// Затемнение вспомогательных форм
        /// </summary>
        public Visibility SSkey
        {
            get
            {
                return _SSkey;
            }
            set
            {
                if (_SSkey != value)
                {
                    _SSkey = value;
                    OnPropertyChanged("SSkey");
                }
            }
        }

        //Анимация
        public Visibility _Akey;
        /// <summary>
        /// Анимация
        /// </summary>
        public Visibility Akey
        {
            get
            {
                return _Akey;
            }
            set
            {
                if (_Akey != value)
                {
                    _Akey = value;
                    OnPropertyChanged("Akey");
                }
            }
        }

        //Сообщение об ошибке
        public Visibility _Mkey;
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public Visibility Mkey
        {
            get
            {
                return _Mkey;
            }
            set
            {
                if (_Mkey != value)
                {
                    _Mkey = value;
                    OnPropertyChanged("Mkey");
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
