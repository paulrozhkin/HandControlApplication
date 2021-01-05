// --------------------------------------------------------------------------------------
// <copyright file = "GestureModel.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
    public class GestureModel : BaseModel, ICloneable, IBinarySerialize, IEquatable<GestureModel>
    {
        #region Fields

        /// <summary>
        ///     Название жеста. Выступает в качестве идентификатора в системе.
        /// </summary>
        private string name = string.Empty;

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
        public string Name
        {
            get => name;

            set
            {
                var lastName = name;
                name = value;
            }
        }

        /// <summary>
        ///     Gets or sets информацию о жесте, такую как время создания/изменения жеста, кол-во действий, кол-во повторений
        ///     действия и итеративность действий.
        /// </summary>
        public InfoGestureModel InfoGesture { get; set; }

        /// <summary>
        ///     Gets or sets список действий жеста.
        /// </summary>
        public List<MotionModel> ListMotions { get; set; }

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
                ListMotions = new List<MotionModel>()
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
            var newDataMotion = new List<MotionModel>();

            if (ListMotions != null)
                foreach (var action in ListMotions)
                    newDataMotion.Add((MotionModel) action.Clone());

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
            ////hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.Name);
            hashCode = hashCode * -1521134295 + InfoGesture.GetHashCode();
            for (var i = 0; i < ListMotions.Count; i++)
                hashCode = hashCode * -1521134295 + ListMotions[i].GetHashCode();

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
        ///     Выполняет сериализацию экземпляра <see cref="GestureModel" /> в бинарный формат.
        /// </summary>
        /// <returns>Экземпляр <see cref="GestureModel" />, представленный в виде бинарного потока.</returns>
        public byte[] BinarySerialize()
        {
            using (var m = new MemoryStream())
            {
                using (var writer = new BinaryWriter(m))
                {
                    writer.Write(Id.ToByteArray());
                    var nameBts = Encoding.UTF8.GetBytes(Name);
                    writer.Write((byte) nameBts.Length);
                    writer.Write(nameBts);
                    writer.Write(InfoGesture.BinarySerialize());

                    if (InfoGesture.NumberOfMotions != ListMotions.Count)
                        throw new ArgumentException("NumberOfMotions and ListMotions count do not match.");
                    for (var i = 0; i < InfoGesture.NumberOfMotions; i++)
                        writer.Write(ListMotions[i].BinarySerialize());
                }

                return m.ToArray();
            }
        }

        /// <summary>
        ///     Выполняет десериализацию экземпляра <see cref="GestureModel" /> из бинарного потока.
        /// </summary>
        /// <param name="data">Бинарный поток.</param>
        public void BinaryDesserialize(byte[] data)
        {
            using (var m = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(m))
                {
                    Id = new Guid(reader.ReadBytes(16));
                    int lengthName = reader.ReadByte();
                    Name = Encoding.UTF8.GetString(reader.ReadBytes(lengthName));
                    InfoGesture.BinaryDesserialize(reader.ReadBytes(11));

                    ListMotions.Clear();
                    for (var i = 0; i < InfoGesture.NumberOfMotions; i++)
                    {
                        var motion = MotionModel.GetDefault(i);
                        motion.BinaryDesserialize(reader.ReadBytes(9));
                        ListMotions.Add(motion);
                    }
                }
            }
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
        public class MotionModel : BaseModel, ICloneable, IBinarySerialize, IEquatable<MotionModel>
        {
            #region Constructors

            /// <summary>
            ///     Prevents a default instance of the <see cref="MotionModel" /> class from being created.
            /// </summary>
            private MotionModel()
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
            public int ThumbFinger { get; set; }

            /// <summary>
            ///     Gets or sets положение указательного пальца в градусах.
            /// </summary>
            public int PointerFinger { get; set; }

            /// <summary>
            ///     Gets or sets положение среднего пальца в градусах.
            /// </summary>
            public int MiddleFinger { get; set; }

            /// <summary>
            ///     Gets or sets положение безымянного пальца в градусах.
            /// </summary>
            public int RingFinder { get; set; }

            /// <summary>
            ///     Gets or sets положение мезинца в градусах.
            /// </summary>
            public int LittleFinger { get; set; }

            /// <summary>
            ///     Gets or sets положение кисти в градусах.
            /// </summary>
            public int StatePosBrush { get; set; }

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

            public static bool operator ==(MotionModel motion, MotionModel other)
            {
                var isVehicleNull = motion is null;
                var isOtherNull = other is null;

                if (isVehicleNull && isOtherNull)
                    return true;
                if (isVehicleNull)
                    return false;
                return motion.Equals(other);
            }

            public static bool operator !=(MotionModel motion, MotionModel other)
            {
                return !(motion == other);
            }

            /// <summary>
            ///     Фабричный метод для получения экземпляра MotionModel с дефолтными параметрами.
            ///     Для создания экземпляра требуется передача Id действия.
            /// </summary>
            /// <param name="idMotion">Id действия</param>
            /// <returns>Новый уникальный идентификатор действия.</returns>
            public static MotionModel GetDefault(int idMotion)
            {
                var result = new MotionModel
                {
                    Id = idMotion,
                    ThumbFinger = 0,
                    PointerFinger = 0,
                    MiddleFinger = 0,
                    RingFinder = 0,
                    LittleFinger = 0,
                    DelMotion = 0,
                    StatePosBrush = 0
                };
                return result;
            }

            /// <summary>
            ///     Генерация нового Id единичного действия на основании коллекции имеющихся действий жеста.
            /// </summary>
            /// <param name="listMotions">Коллекция имеющихся действий в жесте.</param>
            /// <returns>Коллекция действий жеста.</returns>
            public static int GetNewId(List<MotionModel> listMotions)
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
            ///     Выполняет сериализацию экземпляра <see cref="MotionModel" /> в бинарный формат.
            /// </summary>
            /// <returns>Экземпляр <see cref="MotionModel" />, представленный в виде бинарного потока.</returns>
            public byte[] BinarySerialize()
            {
                using (var m = new MemoryStream())
                {
                    using (var writer = new BinaryWriter(m))
                    {
                        writer.Write((byte) PointerFinger);
                        writer.Write((byte) MiddleFinger);
                        writer.Write((byte) RingFinder);
                        writer.Write((byte) LittleFinger);
                        writer.Write((byte) ThumbFinger);
                        writer.Write((ushort) StatePosBrush);
                        writer.Write((ushort) DelMotion);
                    }

                    return m.ToArray();
                }
            }

            /// <summary>
            ///     Выполняет десериализацию экземпляра <see cref="MotionModel" /> из бинарного потока.
            /// </summary>
            /// <param name="data">Бинарный поток.</param>
            public void BinaryDesserialize(byte[] data)
            {
                using (var m = new MemoryStream(data))
                {
                    using (var reader = new BinaryReader(m))
                    {
                        PointerFinger = reader.ReadByte();
                        MiddleFinger = reader.ReadByte();
                        RingFinder = reader.ReadByte();
                        LittleFinger = reader.ReadByte();
                        ThumbFinger = reader.ReadByte();
                        StatePosBrush = reader.ReadUInt16();
                        DelMotion = reader.ReadUInt16();
                    }
                }
            }

            /// <summary>
            ///     Полное клонирование экземпляра MotionModel.
            /// </summary>
            /// <returns>Клонированный экземпляр MotionModel.</returns>
            public object Clone()
            {
                var result = new MotionModel
                {
                    Id = Id,
                    ThumbFinger = ThumbFinger,
                    PointerFinger = PointerFinger,
                    MiddleFinger = MiddleFinger,
                    RingFinder = RingFinder,
                    LittleFinger = LittleFinger,
                    DelMotion = DelMotion,
                    StatePosBrush = StatePosBrush
                };
                return result;
            }

            /// <summary>
            ///     Получить хэш код экземпляра класса<see cref="MotionModel" />.
            /// </summary>
            /// <returns>HashCode экземпляра<see cref="MotionModel" />.</returns>
            public override int GetHashCode()
            {
                var hashCode = 1587829154;
                return hashCode;
            }

            /// <summary>
            ///     Сравнение экземпляра класса <see cref="MotionModel" /> с передаваемым объектом.
            /// </summary>
            /// <param name="obj">Передаваемый объект.</param>
            /// <returns>True, если экземпляры равны.</returns>
            public override bool Equals(object obj)
            {
                return Equals(obj as MotionModel);
            }

            /// <summary>
            ///     Сравнение двух экземпляров класса <see cref="MotionModel" />.
            /// </summary>
            /// <param name="other">Экземпляр класса <see cref="MotionModel" /> для сравнения.</param>
            /// <returns>True, если экземпляры равны.</returns>
            public bool Equals(MotionModel other)
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
                           ReferenceEquals(StatePosBrush, other.StatePosBrush) ||
                           StatePosBrush.Equals(other.StatePosBrush))
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
        public class InfoGestureModel : BaseModel, ICloneable, IBinarySerialize, IEquatable<InfoGestureModel>
        {
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
            public int NumberOfGestureRepetitions { get; set; }

            /// <summary>
            ///     Gets or sets количество действий в жесте.
            /// </summary>
            public int NumberOfMotions { get; set; }

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
                    NumberOfMotions = 0
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
            ///     Выполняет преобразование текущей даты и времени изменения в Unix время.
            /// </summary>
            /// <returns>Unix time.</returns>
            private double TimeChangeToUnix()
            {
                var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                var diff = TimeChange.ToUniversalTime() - origin;
                return Math.Floor(diff.TotalSeconds);
            }

            /// <summary>
            ///     Выполняет установку последнего времени изменения из unix времени.
            /// </summary>
            private void TimeChangeFromUnix(double dateTime)
            {
                var outer = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                TimeChange = outer.AddSeconds(dateTime).ToLocalTime();
            }

            /// <summary>
            ///     Выполняет сериализацию экземпляра <see cref="InfoGestureModel" /> в бинарный формат.
            /// </summary>
            /// <returns>Экземпляр <see cref="InfoGestureModel" />, представленный в виде бинарного потока.</returns>
            public byte[] BinarySerialize()
            {
                using (var m = new MemoryStream())
                {
                    using (var writer = new BinaryWriter(m))
                    {
                        //double unixTime = (this.TimeChange - new DateTime(1970, 1, 1)).TotalSeconds;

                        writer.Write((uint) TimeChangeToUnix());
                        writer.Write(Convert.ToByte(IterableGesture));
                        writer.Write((byte) NumberOfGestureRepetitions);
                        writer.Write((byte) NumberOfMotions);
                    }

                    return m.ToArray();
                }
            }

            /// <summary>
            ///     Выполняет десериализацию экземпляра <see cref="InfoGestureModel" /> из бинарного потока.
            /// </summary>
            /// <param name="data">Бинарный поток.</param>
            public void BinaryDesserialize(byte[] data)
            {
                using (var m = new MemoryStream(data))
                {
                    using (var reader = new BinaryReader(m))
                    {
                        TimeChangeFromUnix(reader.ReadUInt32());
                        IterableGesture = Convert.ToBoolean(reader.ReadByte());
                        NumberOfGestureRepetitions = reader.ReadByte();
                        NumberOfMotions = reader.ReadByte();
                    }
                }
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
                ////hashCode = (hashCode * -1521134295) + this.IterableGesture.GetHashCode();
                ////hashCode = (hashCode * -1521134295) + this.NumberOfGestureRepetitions.GetHashCode();
                ////hashCode = (hashCode * -1521134295) + this.NumberOfMotions.GetHashCode();
                ////hashCode = (hashCode * -1521134295) + EqualityComparer<DateTime>.Default.GetHashCode(this.TimeChange);
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
                           NumberOfGestureRepetitions.Equals(other.NumberOfGestureRepetitions))
                       && (
                           ReferenceEquals(NumberOfMotions, other.NumberOfMotions) ||
                           NumberOfMotions.Equals(other.NumberOfMotions));
            }

            #endregion
        }

        #endregion
    }
}