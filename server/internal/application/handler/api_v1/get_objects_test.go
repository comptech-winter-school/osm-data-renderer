package api_v1

/*

func TestLGetObjects(t *testing.T) {
	type FuncTest struct {
		Arg         []Point
		Expected    string
		ExpectedErr error
	}

	funcTest := []FuncTest{
		{[]Point{{1, 2}, {3, 4}}, "LINESTRING(1.000000 2.000000,3.000000 4.000000)", nil},
		{[]Point{{1.312, 2.2342342234}}, "LINESTRING(1.312000 2.234234)", nil},
		{[]Point{}, "LINESTRING()", nil},
	}

	for _, test := range funcTest {
		got, gotErr := LineToLineString(test.Arg)
		if gotErr != test.ExpectedErr {
			t.Error()
		}

		if got != test.Expected {
			t.Error()
		}
	}
}
*/
