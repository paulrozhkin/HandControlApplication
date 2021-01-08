// --------------------------------------------------------------------------------------
// <copyright file = "IBinarySerialize.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------

namespace HandControl.Model.Dto
{
    /// <summary>
    ///     Интерфейс предназначенный для бинарной сериализации экземпляров.
    /// </summary>
    public interface IBinarySerialize
    {
        /// <summary>
        ///     Выполняет сериализацию экземпляра в бинарный массив.
        /// </summary>
        /// <returns>Экземпляр, представленный в виде бинарного массива.</returns>
        byte[] BinarySerialize();

        /// <summary>
        ///     Выполняет десериализацию экземпляра из бинарного массива.
        /// </summary>
        /// <param name="data">Бинарный массив.</param>
        void BinaryDeserialize(byte[] data);
    }
}