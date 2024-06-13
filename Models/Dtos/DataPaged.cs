namespace AplicacaoWeb.Models.Dtos
{
    public class DataPaged<T>
    {
        public DataPaged(int size, T data)
        {
            Data = data;
            Size = size;
        }
        public T Data { get; set; }
        public int Size { get; set; }
    } 
}
