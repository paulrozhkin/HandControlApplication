// --------------------------------------------------------------------------------------
// <copyright file = "GestureModel.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------
namespace HandControl.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using HandControl.Services;

    /// <summary>
    /// Класс содержащий целостный жест (некоторое действие или движение протеза, имеющее определённое значение или смысл) протеза.
    /// Экземпляр данного класса содержит информацию о комманде и положения принимаемые протезом в разные единицы времени.
    /// Содержит методы для сохранения и загрузки данных.
    /// \brief Класс содержащий целостный жест протеза.
    /// \version 1.1
    /// \date Март 2019 года
    /// \authors Paul Rozhkin(blackiiifox@gmail.com)
    /// </summary>
    public class GestureModel : BaseModel, ICloneable, IBinarySerialize, IEquatable<GestureModel>
    {
        #region Fields
        /// <summary>
        /// Название жеста. Выступает в качестве идентификатора в системе. 
        /// </summary>
        private string name = string.Empty;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GestureModel" /> class.
        /// </summary>
        /// <param name="id">Id жеста.</param>
        /// <param name="nameGesture">Имя жеста.</param>
        private GestureModel(Guid id, string nameGesture)
        {
            this.ID = id;
            this.Name = nameGesture;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="GestureModel" /> class from being created.
        /// </summary>
        private GestureModel()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets уникальный идентификатор жеста.
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// Gets or sets имя жеста, должно быть уникальным.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                string lastName = this.name;
                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets информацию о жесте, такую как время создания/изменения жеста, кол-во действий, кол-во повторений действия и итеративность действий.
        /// </summary>
        public InfoGestureModel InfoGesture { get; set; }

        /// <summary>
        /// Gets or sets список действий жеста.
        /// </summary>
        public List<MotionModel> ListMotions { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Фабричный метод для получения экземпляра <see cref="GestureModel"/> с дефолтными параметрами, но с уникальным именем.
        /// </summary>
        /// <param name="id">Id жеста.</param>
        /// <param name="nameGesture">Имя жеста.</param>
        /// <returns>Экземпляра <see cref="GestureModel"/>.</returns>
        public static GestureModel GetDefault(Guid id, string nameGesture)
        {
            GestureModel result = new GestureModel(id, nameGesture)
            {
                InfoGesture = InfoGestureModel.GetDefault(),
                ListMotions = new List<MotionModel>()
            };

            return result;
        }

        /// <summary>
        /// Извлечение списка жестов системы.
        /// </summary>
        /// <returns>Коллекция жестов хранимых в системе.</returns>
        public static ObservableCollection<GestureModel> GetGestures()
        {
            ObservableCollection<GestureModel> sessionLoaded = new ObservableCollection<GestureModel>();
            foreach (var item in PathManager.GetGesturesFilesPaths())
            {
                GestureModel loadedCommand = (GestureModel)JsonSerDer.LoadObject<GestureModel>(item);

                if (loadedCommand.InfoGesture == null)
                {
                    loadedCommand.InfoGesture = InfoGestureModel.GetDefault();
                }

                sessionLoaded.Add(loadedCommand);
            }

            return sessionLoaded;
        }

        /// <summary>
        /// Полное клонирование экземпляра CommandModel.
        /// </summary>
        /// <returns>Клонированный экземпляр CommandModel.</returns>
        public object Clone()
        {
            var newDataMotion = new List<MotionModel>();

            if (this.ListMotions != null)
            {
                foreach (var action in this.ListMotions)
                {
                    newDataMotion.Add((MotionModel)action.Clone());
                }
            }

            return new GestureModel(this.ID, (string)this.Name.Clone())
            {
                InfoGesture = (InfoGestureModel)this.InfoGesture.Clone(),
                ListMotions = newDataMotion
            };
        }

        /// <summary>
        /// Получить хэш код экземпляра класса<see cref="GestureModel"/>.
        /// </summary>
        /// <returns>HashCode экземпляра<see cref= "GestureModel"/>.</returns>
        public override int GetHashCode()
        {
            var hashCode = -677228334;
            hashCode = (hashCode * -1521134295) + this.ID.GetHashCode();
            ////hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.Name);
            hashCode = (hashCode * -1521134295) + this.InfoGesture.GetHashCode();
            for (int i = 0; i < this.ListMotions.Count; i++)
            {
                hashCode = (hashCode * -1521134295) + this.ListMotions[i].GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Сравнение экземпляра класса <see cref="GestureModel"/> с передаваемым объектом.
        /// </summary>
        /// <param name="obj">Передаваемый объект.</param>
        /// <returns>True, если экземпляры равны.</returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as GestureModel);
        }

        public static bool operator ==(GestureModel gesture, GestureModel other)
        {

            bool isVehicleNull = gesture is null;
            bool isOtherNull = other is null;

            if (isVehicleNull && isOtherNull)
            {
                return true;
            }
            else if (isVehicleNull)
            {
                return false;
            }
            else
            {
                return gesture.Equals(other);
            }
        }

        public static bool operator !=(GestureModel gesture, GestureModel other)
        {
            return !(gesture == other);
        }

        /// <summary>
        /// Выполняет сериализацию экземпляра <see cref="GestureModel"/> в бинарный формат.
        /// </summary>
        /// <returns>Экземпляр <see cref="GestureModel"/>, представленный в виде бинарного потока.</returns>
        public byte[] BinarySerialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(this.ID.ToByteArray());
                    byte[] nameBts = Encoding.UTF8.GetBytes(this.Name);
                    writer.Write((byte)nameBts.Length);
                    writer.Write(nameBts);
                    writer.Write(this.InfoGesture.BinarySerialize());

                    if (this.InfoGesture.NumberOfMotions != this.ListMotions.Count)
                    {
                        throw new ArgumentException("NumberOfMotions and ListMotions count do not match.");
                    }
                    else
                    {
                        for (int i = 0; i < this.InfoGesture.NumberOfMotions; i++)
                        {
                            writer.Write(this.ListMotions[i].BinarySerialize());
                        }
                    }
                }

                return m.ToArray();
            }
        }

        /// <summary>
        /// Выполняет десериализацию экземпляра <see cref="GestureModel"/> из бинарного потока.
        /// </summary>
        /// <param name="data">Бинарный поток.</param>
        public void BinaryDesserialize(byte[] data)
        {
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    this.ID = new Guid(reader.ReadBytes(16));
                    int lengthName = reader.ReadByte();
                    this.Name = Encoding.UTF8.GetString(reader.ReadBytes(lengthName));
                    this.InfoGesture.BinaryDesserialize(reader.ReadBytes(11));

                    this.ListMotions.Clear();
                    for (int i = 0; i < this.InfoGesture.NumberOfMotions; i++)
                    {
                        MotionModel motion = MotionModel.GetDefault(i);
                        motion.BinaryDesserialize(reader.ReadBytes(8));
                        this.ListMotions.Add(motion);
                    }
                }
            }
        }

       //bool IEquatable<GestureModel>.Equals(GestureModel other)
       // {
       //     throw new NotImplementedException();
       // }

        /// <summary>
        /// Сравнение двух экземпляров класса <see cref="GestureModel"/>.
        /// </summary>
        /// <param name="other">Экземпляр класса <see cref="GestureModel"/> для сравнения.</param>
        /// <returns>True, если экземпляры равны.</returns>
        public bool Equals(GestureModel other)
        {
            if (other == null)
            {
                return false;
            }

            return (
                object.ReferenceEquals(this.Name, other.Name) ||
                (this.Name != null &&
                this.Name.Equals(other.Name)))
                && (
                object.ReferenceEquals(this.ID, other.ID) ||
                this.ID.Equals(other.ID))
                && (
                object.ReferenceEquals(this.InfoGesture, other.InfoGesture) ||
                (this.InfoGesture != null &&
                this.InfoGesture.Equals(other.InfoGesture)))
                && (
                object.ReferenceEquals(this.ListMotions, other.ListMotions) ||
                (this.ListMotions != null &&
                this.ListMotions.SequenceEqual(other.ListMotions)));
        }
        #endregion

        #region Classes
        /// <summary>
        /// Класс содержащий единичное положение протеза.
        /// </summary>
        public class MotionModel : BaseModel, ICloneable, IBinarySerialize
        {
            #region Constructors
            /// <summary>
            /// Prevents a default instance of the <see cref="MotionModel" /> class from being created.
            /// </summary>
            private MotionModel()
            {
            }
            #endregion

            #region Propeties
            /// <summary>
            /// Gets or sets номер действия.
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Gets or sets положение большого пальца в градусах.
            /// </summary>
            public int ThumbFinger { get; set; }

            /// <summary>
            /// Gets or sets положение указательного пальца в градусах.
            /// </summary>
            public int PointerFinger { get; set; }

            /// <summary>
            /// Gets or sets положение среднего пальца в градусах.
            /// </summary>
            public int MiddleFinger { get; set; }

            /// <summary>
            /// Gets or sets положение безымянного пальца в градусах.
            /// </summary>
            public int RingFinder { get; set; }

            /// <summary>
            /// Gets or sets положение мезинца в градусах.
            /// </summary>
            public int LittleFinger { get; set; }

            /// <summary>
            /// Gets or sets положение кисти в градусах.
            /// </summary>
            public int StatePosBrush { get; set; }

            /// <summary>
            /// Gets or sets задержка между действиями в секундах.
            /// </summary>
            public double DelMotionSec
            {
                get
                {
                    return this.DelMotion / 10.0;
                }

                set
                {
                    if (value < 0)
                    {
                        value = 0;
                    }

                    if (value > 10)
                    {
                        value = 10;
                    }

                    this.DelMotion = (int)(value * 10);
                }
            }

            /// <summary>
            /// Gets or sets задержку между действиями в милисекундах.
            /// </summary>
            public int DelMotion { get; set; }
            #endregion

            #region Methods
            /// <summary>
            /// Фабричный метод для получения экземпляра MotionModel с дефолтными параметрами.
            /// Для создания экземпляра требуется передача Id действия.
            /// </summary>
            /// <param name="idMotion">Id действия</param>
            /// <returns>Новый уникальный идентификатор действия.</returns>
            public static MotionModel GetDefault(int idMotion)
            {
                MotionModel result = new MotionModel()
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
            /// Генерация нового Id единичного действия на основании коллекции имеющихся действий жеста.
            /// </summary>
            /// <param name="listMotions">Коллекция имеющихся действий в жесте.</param>
            /// <returns>Коллекция действий жеста.</returns>
            public static int GetNewId(List<MotionModel> listMotions)
            {
                int maxId = 0;

                for (int i = 0; i < listMotions.Count; i++)
                {
                    if (listMotions[i].Id > maxId)
                    {
                        maxId = listMotions[i].Id;
                    }
                }

                for (int i = 1; i < maxId; i++)
                {
                    bool state_search = false;
                    for (int j = 0; j < listMotions.Count; j++)
                    {
                        if (listMotions[j].Id == i)
                        {
                            state_search = true;
                            break;
                        }
                    }

                    if (state_search == false)
                    {
                        int newId = i;
                        return newId;
                    }
                }

                return maxId + 1;
            }

            /// <summary>
            /// Выполняет сериализацию экземпляра <see cref="MotionModel"/> в бинарный формат.
            /// </summary>
            /// <returns>Экземпляр <see cref="MotionModel"/>, представленный в виде бинарного потока.</returns>
            public byte[] BinarySerialize()
            {
                using (MemoryStream m = new MemoryStream())
                {
                    using (BinaryWriter writer = new BinaryWriter(m))
                    {
                        writer.Write((byte)this.PointerFinger);
                        writer.Write((byte)this.MiddleFinger);
                        writer.Write((byte)this.RingFinder);
                        writer.Write((byte)this.LittleFinger);
                        writer.Write((byte)this.ThumbFinger);
                        writer.Write((byte)this.StatePosBrush);
                        writer.Write((ushort)this.DelMotion);
                    }

                    return m.ToArray();
                }
            }

            /// <summary>
            /// Выполняет десериализацию экземпляра <see cref="MotionModel"/> из бинарного потока.
            /// </summary>
            /// <param name="data">Бинарный поток.</param>
            public void BinaryDesserialize(byte[] data)
            {
                using (MemoryStream m = new MemoryStream(data))
                {
                    using (BinaryReader reader = new BinaryReader(m))
                    {
                        this.PointerFinger = reader.ReadByte();
                        this.MiddleFinger = reader.ReadByte();
                        this.RingFinder = reader.ReadByte();
                        this.LittleFinger = reader.ReadByte();
                        this.ThumbFinger = reader.ReadByte();
                        this.StatePosBrush = reader.ReadByte();
                        this.DelMotion = reader.ReadUInt16();
                    }
                }
            }

            /// <summary>
            /// Полное клонирование экземпляра MotionModel.
            /// </summary>
            /// <returns>Клонированный экземпляр MotionModel.</returns>
            public object Clone()
            {
                MotionModel result = new MotionModel()
                {
                    Id = this.Id,
                    ThumbFinger = this.ThumbFinger,
                    PointerFinger = this.PointerFinger,
                    MiddleFinger = this.MiddleFinger,
                    RingFinder = this.RingFinder,
                    LittleFinger = this.LittleFinger,
                    DelMotion = this.DelMotion,
                    StatePosBrush = this.StatePosBrush
                };
                return result;
            }

            /// <summary>
            /// Получить хэш код экземпляра класса<see cref="MotionModel"/>.
            /// </summary>
            /// <returns>HashCode экземпляра<see cref= "MotionModel"/>.</returns>
            public override int GetHashCode()
            {
                var hashCode = 1587829154;
                ////hashCode = (hashCode * -1521134295) + this.Id.GetHashCode();
                ////hashCode = (hashCode * -1521134295) + this.ThumbFinger.GetHashCode();
                ////hashCode = (hashCode * -1521134295) + this.PointerFinger.GetHashCode();
                ////hashCode = (hashCode * -1521134295) + this.MiddleFinger.GetHashCode();
                ////hashCode = (hashCode * -1521134295) + this.RingFinder.GetHashCode();
                ////hashCode = (hashCode * -1521134295) + this.LittleFinger.GetHashCode();
                ////hashCode = (hashCode * -1521134295) + this.StatePosBrush.GetHashCode();
                ////hashCode = (hashCode * -1521134295) + this.DelMotion.GetHashCode();
                return hashCode;
            }

            /// <summary>
            /// Сравнение экземпляра класса <see cref="MotionModel"/> с передаваемым объектом.
            /// </summary>
            /// <param name="obj">Передаваемый объект.</param>
            /// <returns>True, если экземпляры равны.</returns>
            public override bool Equals(object obj)
            {
                return this.Equals(obj as MotionModel);
            }

            public static bool operator ==(MotionModel motion, MotionModel other)
            {

                bool isVehicleNull = motion is null;
                bool isOtherNull = other is null;

                if (isVehicleNull && isOtherNull)
                {
                    return true;
                }
                else if (isVehicleNull)
                {
                    return false;
                }
                else
                {
                    return motion.Equals(other);
                }
            }

            public static bool operator !=(MotionModel motion, MotionModel other)
            {
                return !(motion == other);
            }

            /// <summary>
            /// Сравнение двух экземпляров класса <see cref="MotionModel"/>.
            /// </summary>
            /// <param name="other">Экземпляр класса <see cref="MotionModel"/> для сравнения.</param>
            /// <returns>True, если экземпляры равны.</returns>
            private bool Equals(MotionModel other)
            {
                if (other == null)
                {
                    return false;
                }

                return (
                   object.ReferenceEquals(this.Id, other.Id) ||
                   this.Id.Equals(other.Id))
                   && (
                   object.ReferenceEquals(this.LittleFinger, other.LittleFinger) ||
                   this.LittleFinger.Equals(other.LittleFinger))
                   && (
                   object.ReferenceEquals(this.MiddleFinger, other.MiddleFinger) ||
                   this.MiddleFinger.Equals(other.MiddleFinger))
                   && (
                   object.ReferenceEquals(this.PointerFinger, other.PointerFinger) ||
                   this.PointerFinger.Equals(other.PointerFinger))
                   && (
                   object.ReferenceEquals(this.RingFinder, other.RingFinder) ||
                   this.RingFinder.Equals(other.RingFinder))
                   && (
                   object.ReferenceEquals(this.StatePosBrush, other.StatePosBrush) ||
                   this.StatePosBrush.Equals(other.StatePosBrush))
                   && (
                   object.ReferenceEquals(this.ThumbFinger, other.ThumbFinger) ||
                   this.ThumbFinger.Equals(other.ThumbFinger))
                   && (
                   object.ReferenceEquals(this.DelMotion, other.DelMotion) ||
                   this.DelMotion.Equals(other.DelMotion));
            }
            #endregion
        }

        /// <summary>
        /// Класс содержащий информацию о жесте <see cref="GestureModel"/>.
        /// </summary>
        public class InfoGestureModel : BaseModel, ICloneable, IBinarySerialize
        {
            #region Constructors
            /// <summary>
            /// Prevents a default instance of the <see cref="InfoGestureModel" /> class from being created.
            /// </summary>
            private InfoGestureModel()
            {
            }
            #endregion

            #region Properties
            /// <summary>
            /// Gets or sets a value indicating whether итерируемость жеста.
            /// </summary>
            public bool IterableGesture { get; set; }

            /// <summary>
            /// Gets or sets количество повторений жеста.
            /// </summary>
            public int NumberOfGestureRepetitions { get; set; }

            /// <summary>
            /// Gets or sets количество действий в жесте.
            /// </summary>
            public int NumberOfMotions { get; set; }

            /// <summary>
            /// Gets or sets время последнего изменения/создания жеста.
            /// </summary>
            public DateTime TimeChange { get; set; }
            #endregion

            #region Methods
            /// <summary>
            /// Фабричный метод для получения экземпляра InfoCommandModel с дефолтными параметрами.
            /// </summary>
            /// <returns>Экземпляр InfoCommandModel.</returns>
            public static InfoGestureModel GetDefault()
            {
                InfoGestureModel result = new InfoGestureModel()
                {
                    TimeChange = DateTime.Now,
                    IterableGesture = false,
                    NumberOfGestureRepetitions = 1,
                    NumberOfMotions = 0
                };
                return result;
            }

            /// <summary>
            /// Выполняет сериализацию экземпляра <see cref="InfoGestureModel"/> в бинарный формат.
            /// </summary>
            /// <returns>Экземпляр <see cref="InfoGestureModel"/>, представленный в виде бинарного потока.</returns>
            public byte[] BinarySerialize()
            {
                using (MemoryStream m = new MemoryStream())
                {
                    using (BinaryWriter writer = new BinaryWriter(m))
                    {
                        //double unixTime = (this.TimeChange.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
                        double unixTime = (this.TimeChange- new DateTime(1970, 1, 1)).TotalSeconds;
                        writer.Write(unixTime);
                        writer.Write(Convert.ToByte(this.IterableGesture));
                        writer.Write((byte)this.NumberOfGestureRepetitions);
                        writer.Write((byte)this.NumberOfMotions);
                    }

                    return m.ToArray();
                }
            }

            /// <summary>
            /// Выполняет десериализацию экземпляра <see cref="InfoGestureModel"/> из бинарного потока.
            /// </summary>
            /// <param name="data">Бинарный поток.</param>
            public void BinaryDesserialize(byte[] data)
            {
                using (MemoryStream m = new MemoryStream(data))
                {
                    using (BinaryReader reader = new BinaryReader(m))
                    {
                        this.TimeChange = (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddSeconds(reader.ReadDouble());
                        this.IterableGesture = Convert.ToBoolean(reader.ReadByte());
                        this.NumberOfGestureRepetitions = reader.ReadByte();
                        this.NumberOfMotions = reader.ReadByte();
                    }
                }
            }

            /// <summary>
            /// Полное клонирование экземпляра InfoCommandModel.
            /// </summary>
            /// <returns>Клонированный экземпляр InfoCommandModel.</returns>
            public object Clone()
            {
                return this.MemberwiseClone();
            }

            /// <summary>
            /// Получить хэш код экземпляра класса<see cref="InfoGestureModel"/>.
            /// </summary>
            /// <returns>HashCode экземпляра<see cref= "InfoGestureModel"/>.</returns>
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
            /// Сравнение экземпляра класса <see cref="InfoGestureModel"/> с передаваемым объектом.
            /// </summary>
            /// <param name="obj">Передаваемый объект.</param>
            /// <returns>True, если экземпляры равны.</returns>
            public override bool Equals(object obj)
            {
                return this.Equals(obj as InfoGestureModel);
            }

            public static bool operator ==(InfoGestureModel info, InfoGestureModel other)
            {

                bool isVehicleNull = info is null;
                bool isOtherNull = other is null;

                if (isVehicleNull && isOtherNull)
                {
                    return true;
                }
                else if (isVehicleNull)
                {
                    return false;
                }
                else
                {
                    return info.Equals(other);
                }
            }

            public static bool operator !=(InfoGestureModel info, InfoGestureModel other)
            {
                return !(info == other);
            }

            /// <summary>
            /// Сравнение двух экземпляров класса <see cref="InfoGestureModel"/>.
            /// </summary>
            /// <param name="other">Экземпляр класса <see cref="InfoGestureModel"/> для сравнения.</param>
            /// <returns>True, если экземпляры равны.</returns>
            private bool Equals(InfoGestureModel other)
            {
                if (other == null)
                {
                    return false;
                }

                return (
                    object.ReferenceEquals(this.TimeChange, other.TimeChange) ||
                    (this.TimeChange != null &&
                    this.TimeChange.Ticks / 10000000 == other.TimeChange.Ticks / 10000000)) //// Деление, т.к. сравнение проводится только до секунд.
                    && (
                    object.ReferenceEquals(this.IterableGesture, other.IterableGesture) ||
                    this.IterableGesture.Equals(other.IterableGesture))
                    && (
                    object.ReferenceEquals(this.NumberOfGestureRepetitions, other.NumberOfGestureRepetitions) ||
                    this.NumberOfGestureRepetitions.Equals(other.NumberOfGestureRepetitions))
                    && (
                    object.ReferenceEquals(this.NumberOfMotions, other.NumberOfMotions) ||
                    this.NumberOfMotions.Equals(other.NumberOfMotions));
            }
            #endregion
        }
        #endregion
    }
}