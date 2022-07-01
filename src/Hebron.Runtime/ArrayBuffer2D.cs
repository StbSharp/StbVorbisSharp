namespace Hebron.Runtime
{
	internal class ArrayBuffer2D<T>
	{
		public T[] Array { get; private set; }

		public int Capacity1 { get; private set; }

		public int Capacity2 { get; private set; }

		public T this[int index1, int index2]
		{
			get => Array[index1 * Capacity2 + index2];
			set => Array[index1 * Capacity2 + index2] = value;
		}

		public ArrayBuffer2D(int capacity1, int capacity2)
		{
			Capacity1 = capacity1;
			Capacity2 = capacity2;
			Array = new T[capacity1 * capacity2];
		}

		public void EnsureSize(int capacity1, int capacity2)
		{
			Capacity1 = capacity1;
			Capacity2 = capacity2;

			var required = capacity1 * capacity2;
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