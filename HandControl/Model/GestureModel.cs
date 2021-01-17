// --------------------------------------------------------------------------------------
// <copyright file = "GestureModel.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace HandControl.Model
{
    /// <summary>
    ///     Класс содержащий целостный жест (некоторое действие или движение протеза, имеющее определённое значение или смысл)
    ///     протеза.
    ///     Экземпляр данного класса содержит информацию о комманде и положения принимаемые протезом в разные единицы времени.
    ///     Содержит методы для сохранения и загрузки данных.
    ///     \brief Класс содержащий целостный жест протеза.
    ///     \version 1.1
    ///     \date Март 2019 года
    ///     \authors Paul Rozhkin(blackiiifox@gmail.com)
    /// </summary>
    public class GestureModel : BaseModel, ICloneable, IEquatable<GestureModel>
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GestureModel" /> class.
        /// </summary>
        /// <param name="id">Id жеста.</param>
        /// <param name="nameGesture">Имя жеста.</param>
        private GestureModel(Guid id, string nameGesture)
        {
            Id = id;
            Name = nameGesture;
        }

        /// <summary>
        ///     Prevents a default instance of the <see cref="GestureModel" /> class from being created.
        /// </summary>
        private GestureModel()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets уникальный идентификатор жеста.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Gets or sets имя жеста, должно быть уникальным.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets информацию о жесте, такую как время создания/изменения жеста, кол-во действий, кол-во повторений
        ///     действия и итеративность действий.
        /// </summary>
        public InfoGestureModel InfoGesture { get; set; }

        /// <summary>
        ///     Gets or sets список действий жеста.
        /// </summary>
        public List<ActionModel> ListMotions { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Фабричный метод для получения экземпляра <see cref="GestureModel" /> с дефолтными параметрами, но с уникальным
        ///     именем.
        /// </summary>
        /// <param name="id">Id жеста.</param>
        /// <param name="nameGesture">Имя жеста.</param>
        /// <returns>Экземпляра <see cref="GestureModel" />.</returns>
        public static GestureModel GetDefault(Guid id, string nameGesture)
        {
            var result = new GestureModel(id, nameGesture)
            {
                InfoGesture = InfoGestureModel.GetDefault(),
                ListMotions = new List<ActionModel>()
            };

            return result;
        }

        public static bool operator ==(GestureModel gesture, GestureModel other)
        {
            var isVehicleNull = gesture is null;
            var isOtherNull = other is null;

            if (isVehicleNull && isOtherNull)
                return true;
            if (isVehicleNull)
                return false;
            return gesture.Equals(other);
        }

        public static bool operator !=(GestureModel gesture, GestureModel other)
        {
            return !(gesture == other);
        }

        /// <summary>
        ///     Полное клонирование экземпляра CommandModel.
        /// </summary>
        /// <returns>Клонированный экземпляр CommandModel.</returns>
        public object Clone()
        {
            var newDataMotion = new List<ActionModel>();

            if (ListMotions != null)
                foreach (var action in ListMotions)
                    newDataMotion.Add((ActionModel) action.Clone());

            return new GestureModel(Id, (string) Name.Clone())
            {
                InfoGesture = (InfoGestureModel) InfoGesture.Clone(),
                ListMotions = newDataMotion
            };
        }

        /// <summary>
        ///     Получить хэш код экземпляра класса<see cref="GestureModel" />.
        /// </summary>
        /// <returns>HashCode экземпляра<see cref="GestureModel" />.</returns>
        public override int GetHashCode()
        {
            var hashCode = -677228334;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();

            return hashCode;
        }

        /// <summary>
        ///     Сравнение экземпляра класса <see cref="GestureModel" /> с передаваемым объектом.
        /// </summary>
        /// <param name="obj">Передаваемый объект.</param>
        /// <returns>True, если экземпляры равны.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as GestureModel);
        }

        /// <summary>
        ///     Сравнение двух экземпляров класса <see cref="GestureModel" />.
        /// </summary>
        /// <param name="other">Экземпляр класса <see cref="GestureModel" /> для сравнения.</param>
        /// <returns>True, если экземпляры равны.</returns>
        public bool Equals(GestureModel other)
        {
            if (other == null) return false;

            return (
                       ReferenceEquals(Name, other.Name) ||
                       Name != null &&
                       Name.Equals(other.Name))
                   && (
                       ReferenceEquals(Id, other.Id) ||
                       Id.Equals(other.Id))
                   && (
                       ReferenceEquals(InfoGesture, other.InfoGesture) ||
                       InfoGesture != null &&
                       InfoGesture.Equals(other.InfoGesture))
                   && (
                       ReferenceEquals(ListMotions, other.ListMotions) ||
                       ListMotions != null &&
                       ListMotions.SequenceEqual(other.ListMotions));
        }

        #endregion

        #region Classes

        /// <summary>
        ///     Класс содержащий единичное положение протеза.
        /// </summary>
        public class ActionModel : BaseModel, ICloneable, IEquatable<ActionModel>
        {
            private const int MinPosition = 0;
            private const int MaxPosition = 180;
            private int _thumbFinger;
            private int _pointerFinger;
            private int _middleFinger;
            private int _ringFinger;
            private int _littleFinger;

            #region Constructors

            /// <summary>
            ///     Prevents a default instance of the <see cref="ActionModel" /> class from being created.
            /// </summary>
            private ActionModel()
            {
            }

            #endregion

            #region Propeties

            /// <summary>
            ///     Gets or sets номер действия.
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            ///     Gets or sets положение большого пальца в градусах.
            /// </summary>
            public int ThumbFinger
            {
                get => _thumbFinger;
                set
                {
                    if (value < MinPosition)
                    {
                        _thumbFinger = MinPosition;
                        return;
                    }

                    if (value > MaxPosition)
                    {
                        _thumbFinger = MaxPosition;
                        return;
                    }

                    _thumbFinger = value;
                    OnPropertyChanged(nameof(ThumbFinger));
                }
            }

            /// <summary>
            ///     Gets or sets положение указательного пальца в градусах.
            /// </summary>
            public int PointerFinger
            {
                get => _pointerFinger;
                set
                {
                    if (value < MinPosition)
                    {
                        _pointerFinger = MinPosition;
                        return;
                    }

                    if (value > MaxPosition)
                    {
                        _pointerFinger = MaxPosition;
                        return;
                    }

                    _pointerFinger = value;
                    OnPropertyChanged(nameof(PointerFinger));
                }
            }

            /// <summary>
            ///     Gets or sets положение среднего пальца в градусах.
            /// </summary>
            public int MiddleFinger
            {
                get => _middleFinger;
                set
                {
                    if (value < MinPosition)
                    {
                        _middleFinger = MinPosition;
                        return;
                    }

                    if (value > MaxPosition)
                    {
                        _middleFinger = MaxPosition;
                        return;
                    }

                    _middleFinger = value;
                    OnPropertyChanged(nameof(MiddleFinger));
                }
            }

            /// <summary>
            ///     Gets or sets положение безымянного пальца в градусах.
            /// </summary>
            public int RingFinder
            {
                get => _ringFinger;
                set
                {
                    if (value < MinPosition)
                    {
                        _ringFinger = MinPosition;
                        return;
                    }

                    if (value > MaxPosition)
                    {
                        _ringFinger = MaxPosition;
                        return;
                    }

                    _ringFinger = value;
                    OnPropertyChanged(nameof(RingFinder));
                }
            }

            /// <summary>
            ///     Gets or sets положение мезинца в градусах.
            /// </summary>
            public int LittleFinger
            {
                get => _littleFinger;
                set
                {
                    if (value < MinPosition)
                    {
                        _littleFinger = MinPosition;
                        return;
                    }

                    if (value > MaxPosition)
                    {
                        _littleFinger = MaxPosition;
                        return;
                    }

                    _littleFinger = value;
                    OnPropertyChanged(nameof(LittleFinger));
                }
            }

            /// <summary>
            ///     Gets or sets задержка между действиями в секундах.
            /// </summary>
            public double DelMotionSec
            {
                get => DelMotion / 1000.0;

                set
                {
                    if (value < 0) value = 0;

                    if (value > 10) value = 10;

                    DelMotion = (int) (value * 1000);
                }
            }

            /// <summary>
            ///     Gets or sets задержку между действиями в милисекундах.
            /// </summary>
            public int DelMotion { get; set; }

            #endregion

            #region Methods

            public static bool operator ==(ActionModel action, ActionModel other)
            {
                var isVehicleNull = action is null;
                var isOtherNull = other is null;

                if (isVehicleNull && isOtherNull)
                    return true;
                if (isVehicleNull)
                    return false;
                return action.Equals(other);
            }

            public static bool operator !=(ActionModel action, ActionModel other)
            {
                return !(action == other);
            }

            /// <summary>
            ///     Фабричный метод для получения экземпляра ActionModel с дефолтными параметрами.
            ///     Для создания экземпляра требуется передача Id действия.
            /// </summary>
            /// <param name="idMotion">Id действия</param>
            /// <returns>Новый уникальный идентификатор действия.</returns>
            public static ActionModel GetDefault(int idMotion)
            {
                var result = new ActionModel
                {
                    Id = idMotion,
                    ThumbFinger = 0,
                    PointerFinger = 0,
                    MiddleFinger = 0,
                    RingFinder = 0,
                    LittleFinger = 0,
                    DelMotion = 0
                };

                return result;
            }

            /// <summary>
            ///     Генерация нового Id единичного действия на основании коллекции имеющихся действий жеста.
            /// </summary>
            /// <param name="listMotions">Коллекция имеющихся действий в жесте.</param>
            /// <returns>Коллекция действий жеста.</returns>
            public static int GetNewId(List<ActionModel> listMotions)
            {
                var maxId = 0;

                for (var i = 0; i < listMotions.Count; i++)
                    if (listMotions[i].Id > maxId)
                        maxId = listMotions[i].Id;

                for (var i = 1; i < maxId; i++)
                {
                    var state_search = false;
                    for (var j = 0; j < listMotions.Count; j++)
                        if (listMotions[j].Id == i)
                        {
                            state_search = true;
                            break;
                        }

                    if (state_search == false)
                    {
                        var newId = i;
                        return newId;
                    }
                }

                return maxId + 1;
            }

            /// <summary>
            ///     Полное клонирование экземпляра ActionModel.
            /// </summary>
            /// <returns>Клонированный экземпляр ActionModel.</returns>
            public object Clone()
            {
                var result = new ActionModel
                {
                    Id = Id,
                    ThumbFinger = ThumbFinger,
                    PointerFinger = PointerFinger,
                    MiddleFinger = MiddleFinger,
                    RingFinder = RingFinder,
                    LittleFinger = LittleFinger,
                    DelMotion = DelMotion

                };

                return result;
            }

            /// <summary>
            ///     Получить хэш код экземпляра класса<see cref="ActionModel" />.
            /// </summary>
            /// <returns>HashCode экземпляра<see cref="ActionModel" />.</returns>
            public override int GetHashCode()
            {
                var hashCode = 1587829154;
                return hashCode;
            }

            /// <summary>
            ///     Сравнение экземпляра класса <see cref="ActionModel" /> с передаваемым объектом.
            /// </summary>
            /// <param name="obj">Передаваемый объект.</param>
            /// <returns>True, если экземпляры равны.</returns>
            public override bool Equals(object obj)
            {
                return Equals(obj as ActionModel);
            }

            /// <summary>
            ///     Сравнение двух экземпляров класса <see cref="ActionModel" />.
            /// </summary>
            /// <param name="other">Экземпляр класса <see cref="ActionModel" /> для сравнения.</param>
            /// <returns>True, если экземпляры равны.</returns>
            public bool Equals(ActionModel other)
            {
                if (other == null) return false;

                return (
                           ReferenceEquals(Id, other.Id) ||
                           Id.Equals(other.Id))
                       && (
                           ReferenceEquals(LittleFinger, other.LittleFinger) ||
                           LittleFinger.Equals(other.LittleFinger))
                       && (
                           ReferenceEquals(MiddleFinger, other.MiddleFinger) ||
                           MiddleFinger.Equals(other.MiddleFinger))
                       && (
                           ReferenceEquals(PointerFinger, other.PointerFinger) ||
                           PointerFinger.Equals(other.PointerFinger))
                       && (
                           ReferenceEquals(RingFinder, other.RingFinder) ||
                           RingFinder.Equals(other.RingFinder))
                       && (
                           ReferenceEquals(ThumbFinger, other.ThumbFinger) ||
                           ThumbFinger.Equals(other.ThumbFinger))
                       && (
                           ReferenceEquals(DelMotion, other.DelMotion) ||
                           DelMotion.Equals(other.DelMotion));
            }

            #endregion
        }

        /// <summary>
        ///     Класс содержащий информацию о жесте <see cref="GestureModel" />.
        /// </summary>
        public class InfoGestureModel : BaseModel, ICloneable, IEquatable<InfoGestureModel>
        {
            private int _numberOfGestureRepetitions;

            #region Constructors

            /// <summary>
            ///     Prevents a default instance of the <see cref="InfoGestureModel" /> class from being created.
            /// </summary>
            private InfoGestureModel()
            {
            }

            #endregion

            #region Properties

            /// <summary>
            ///     Gets or sets a value indicating whether итерируемость жеста.
            /// </summary>
            public bool IterableGesture { get; set; }

            /// <summary>
            ///     Gets or sets количество повторений жеста.
            /// </summary>
            public int NumberOfGestureRepetitions
            {
                get => _numberOfGestureRepetitions;
                set
                {
                    if (value < 0)
                    {
                        _numberOfGestureRepetitions = 0;
                        return;
                    }

                    if (value > 255)
                    {
                        _numberOfGestureRepetitions = 255;
                        return;
                    }

                    _numberOfGestureRepetitions = value;
                    OnPropertyChanged(nameof(NumberOfGestureRepetitions));
                }
            }

            /// <summary>
            ///     Gets or sets время последнего изменения/создания жеста.
            /// </summary>
            public DateTime TimeChange { get; set; }

            #endregion

            #region Methods

            /// <summary>
            ///     Фабричный метод для получения экземпляра InfoCommandModel с дефолтными параметрами.
            /// </summary>
            /// <returns>Экземпляр InfoCommandModel.</returns>
            public static InfoGestureModel GetDefault()
            {
                var result = new InfoGestureModel
                {
                    TimeChange = DateTime.Now,
                    IterableGesture = false,
                    NumberOfGestureRepetitions = 1,
                };
                return result;
            }

            public static bool operator ==(InfoGestureModel info, InfoGestureModel other)
            {
                var isVehicleNull = info is null;
                var isOtherNull = other is null;

                if (isVehicleNull && isOtherNull)
                    return true;
                if (isVehicleNull)
                    return false;
                return info.Equals(other);
            }

            public static bool operator !=(InfoGestureModel info, InfoGestureModel other)
            {
                return !(info == other);
            }

            /// <summary>
            ///     Полное клонирование экземпляра InfoCommandModel.
            /// </summary>
            /// <returns>Клонированный экземпляр InfoCommandModel.</returns>
            public object Clone()
            {
                return MemberwiseClone();
            }

            /// <summary>
            ///     Получить хэш код экземпляра класса<see cref="InfoGestureModel" />.
            /// </summary>
            /// <returns>HashCode экземпляра<see cref="InfoGestureModel" />.</returns>
            public override int GetHashCode()
            {
                var hashCode = 374632536;
                return hashCode;
            }

            /// <summary>
            ///     Сравнение экземпляра класса <see cref="InfoGestureModel" /> с передаваемым объектом.
            /// </summary>
            /// <param name="obj">Передаваемый объект.</param>
            /// <returns>True, если экземпляры равны.</returns>
            public override bool Equals(object obj)
            {
                return Equals(obj as InfoGestureModel);
            }

            /// <summary>
            ///     Сравнение двух экземпляров класса <see cref="InfoGestureModel" />.
            /// </summary>
            /// <param name="other">Экземпляр класса <see cref="InfoGestureModel" /> для сравнения.</param>
            /// <returns>True, если экземпляры равны.</returns>
            public bool Equals(InfoGestureModel other)
            {
                if (other == null) return false;

                return (
                           ReferenceEquals(TimeChange, other.TimeChange) ||
                           TimeChange != null &&
                           TimeChange.Ticks / 10000000 == other.TimeChange.Ticks / 10000000
                       ) //// Деление, т.к. сравнение проводится только до секунд.
                       && (
                           ReferenceEquals(IterableGesture, other.IterableGesture) ||
                           IterableGesture.Equals(other.IterableGesture))
                       && (
                           ReferenceEquals(NumberOfGestureRepetitions, other.NumberOfGestureRepetitions) ||
                           NumberOfGestureRepetitions.Equals(other.NumberOfGestureRepetitions));
            }

            #endregion
        }

        #endregion
    }
}