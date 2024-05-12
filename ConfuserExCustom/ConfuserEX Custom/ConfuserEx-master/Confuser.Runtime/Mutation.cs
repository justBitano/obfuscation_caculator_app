using System;

internal class Mutation {
	public static readonly int KeyI0 = 4;
	public static readonly int KeyI1 = 5;
	public static readonly int KeyI2 = 6;
	public static readonly int KeyI3 = 7;
	public static readonly int KeyI4 = 8;
	public static readonly int KeyI5 = 9;
	public static readonly int KeyI6 = 10;
	public static readonly int KeyI7 = 11;
	public static readonly int KeyI8 = 12;
	public static readonly int KeyI9 = 13;
	public static readonly int KeyI10 = 14;
	public static readonly int KeyI11 = 15;
	public static readonly int KeyI12 = 16;
	public static readonly int KeyI13 = 18;
	public static readonly int KeyI14 = 19;
	public static readonly int KeyI15 = 20;

	public static T Placeholder<T>(T val) {
		return val;
	}

	public static T Value<T>() {
		return default(T);
	}

	public static T Value<T, Arg0>(Arg0 arg0) {
		return default(T);
	}

	public static void Crypt(uint[] data, uint[] key) { }
}