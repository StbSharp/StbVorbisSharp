namespace Hebron.Runtime
{
	internal class ArrayBuffer<T>
	{
		public T[] Array { get; private set; }

		public int Capacity => Array.Length;

		public T this[int index]
		{
			get => Array[index];
			set => Array[index] = value;
		}

		public T this[ulong index]
		{
			get => Array[index];
			set => Array[index] = value;
		}

		public ArrayBuffer(int capacity)
		{
			Array = new T[capacity];
		}

		public void EnsureSize(int required)
		{
			if (Array.Length >= required) return;

			// Realloc
			var oldData = Array;

			var newSize = Array.Length;
			while (newSize < required) newSize *= 2;

			Array = new T[newSize];

			System.Array.Copy(oldData, Array, oldData.Length);
		}
	}
}