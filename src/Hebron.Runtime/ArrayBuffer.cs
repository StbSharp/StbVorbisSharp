namespace Hebron.Runtime
{
	internal class ArrayBuffer<T>
	{
		private T[] _array;

		public T[] Array => _array;

		public int Capacity => _array.Length;

		public T this[int index]
		{
			get => _array[index];
			set => _array[index] = value;
		}

		public T this[ulong index]
		{
			get => _array[index];
			set => _array[index] = value;
		}

		public ArrayBuffer(int capacity)
		{
			_array = new T[capacity];
		}

		public void EnsureSize(int required)
		{
			if (_array.Length >= required) return;

			// Realloc
			var oldData = _array;

			var newSize = _array.Length;
			while (newSize < required)
			{
				newSize *= 2;
			}

			_array = new T[newSize];

			System.Array.Copy(oldData, _array, oldData.Length);
		}
	}
}
