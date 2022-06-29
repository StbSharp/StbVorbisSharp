namespace Hebron.Runtime
{
	internal class ArrayBuffer2D<T>
	{
		private T[] _array;
		private int _capacity1 = 0, _capacity2 = 0;

		public T[] Array => _array;

		public int Capacity1 => _capacity1;
		public int Capacity2 => _capacity2;

		public T this[int index1, int index2]
		{
			get => _array[index1 * _capacity2 + index2];
			set => _array[index1 * _capacity2 + index2] = value;
		}

		public ArrayBuffer2D(int capacity1, int capacity2)
		{
			_capacity1 = capacity1;
			_capacity2 = capacity2;
			_array = new T[capacity1 * capacity2];
		}

		public void EnsureSize(int capacity1, int capacity2)
		{
			_capacity1 = capacity1;
			_capacity2 = capacity2;

			var required = capacity1 * capacity2;
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
