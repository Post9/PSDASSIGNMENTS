// 9.1 (i) .NET CIL for SelectionSort, from: dotnet build -c Release, then ilspycmd -il
//
// Argument & Local variables:
// ldarg.0 = arr
// local 0 = i
// local 1 = least
// local 2 = tmp
// local 3 = j

    .method public hidebysig static 
		void SelectionSort (
			int32[] arr
		) cil managed 
	{
		// Method begins at RVA 0x20b8
		// Header size: 12
		// Code size: 57 (0x39)
		.maxstack 4
		.locals init (
			[0] int32, // i
			[1] int32, // least
			[2] int32, // tmp
			[3] int32  // j
		)

		IL_0000: ldc.i4.0      // push 0
		IL_0001: stloc.0       // i = 0
		IL_0002: br.s IL_0032  // Jump to outer loop; test first

		// outer loop body start (head: IL_0032)
			IL_0004: ldloc.0      //push i
			IL_0005: stloc.1      // least = i
			IL_0006: ldloc.0      // push i
			IL_0007: ldc.i4.1     // push 1
			IL_0008: add          // i + 1
			IL_0009: stloc.3      // j = i + 1
			IL_000a: br.s IL_001a // jump to inner loop test

			// inner loop body start (head: IL_001a)
				IL_000c: ldarg.0         // push arr                 
				IL_000d: ldloc.3         // push j
				IL_000e: ldelem.i4       // push arr[j]
				IL_000f: ldarg.0         // push arr
				IL_0010: ldloc.1         // push least
				IL_0011: ldelem.i4       // push arr[least]
				IL_0012: bge.s IL_0016   // if arr[j] >= arr[least], skip

				IL_0014: ldloc.3         // push j
				IL_0015: stloc.1         // least = j

				IL_0016: ldloc.3         // push j
				IL_0017: ldc.i4.1        // push 1
				IL_0018: add             // j + 1
				IL_0019: stloc.3         // j++
                
                // inner loop test
				IL_001a: ldloc.3         // push j
				IL_001b: ldarg.0         // push arr
				IL_001c: ldlen           // push arr.Length
				IL_001d: conv.i4         // convert it to int32
				IL_001e: blt.s IL_000c   // if j < arr.Length, run inner body again
			// end loop

			IL_0020: ldarg.0        // push arr
			IL_0021: ldloc.0        // push i
			IL_0022: ldelem.i4      // push arr[i]
			IL_0023: stloc.2        // tmp = arr[i]
			IL_0024: ldarg.0        // push arr
			IL_0025: ldloc.0        // push i
			IL_0026: ldarg.0        // push arr
			IL_0027: ldloc.1        // push least
			IL_0028: ldelem.i4      // push arr[least]
			IL_0029: stelem.i4      // arr[i] = arr[least]
			IL_002a: ldarg.0        // push arr
			IL_002b: ldloc.1        // push least
			IL_002c: ldloc.2        // push tmp
			IL_002d: stelem.i4      // arr[least] = tmp
			IL_002e: ldloc.0        // push i
			IL_002f: ldc.i4.1       // push 1
			IL_0030: add            // i + 1
			IL_0031: stloc.0        // i++

            // outer loop test
			IL_0032: ldloc.0        // push i
			IL_0033: ldarg.0        // push arr
			IL_0034: ldlen          // push arr.Length
			IL_0035: conv.i4        // convert to int32
			IL_0036: blt.s IL_0004  // if i < arr.Length, run outer body again
		// end loop

		IL_0038: ret  // return (void)
	} // end of method

