using System;
using CPUx86 = Cosmos.Assembler.x86;
using Cosmos.Assembler.x86;
using Cosmos.Assembler.x86.SSE;
using XSharp.Compiler;
using static XSharp.Compiler.XSRegisters;

namespace Cosmos.IL2CPU.X86.IL
{
    [Cosmos.IL2CPU.OpCode( ILOpCode.Code.Add )]
    public class Add : ILOp
    {
        public Add( Cosmos.Assembler.Assembler aAsmblr )
            : base( aAsmblr )
        {
        }

        public override void Execute( MethodInfo aMethod, ILOpCode aOpCode )
        {
            var xType = aOpCode.StackPopTypes[0];
            var xSize = SizeOfType(xType);
            var xIsFloat = TypeIsFloat(xType);
            DoExecute(xSize, xIsFloat);
        }

      public static void DoExecute(uint xSize, bool xIsFloat)
      {
        if (xSize > 8)
        {
          //EmitNotImplementedException( Assembler, aServiceProvider, "Size '" + xSize.Size + "' not supported (add)", aCurrentLabel, aCurrentMethodInfo, aCurrentOffset, aNextLabel );
          throw new NotImplementedException("Cosmos.IL2CPU.x86->IL->Add.cs->Error: StackSize > 8 not supported");
        }
        else
        {
          if (xSize > 4)
          {
            if (xIsFloat)
            {
              XS.FPU.FloatLoad(ESP, destinationIsIndirect: true, size: RegisterSize.Long64);
              XS.Add(ESP, 8);
              new CPUx86.x87.FloatAdd { DestinationReg = CPUx86.RegistersEnum.ESP, DestinationIsIndirect = true, Size = 64 };
              XS.FPU.FloatStoreAndPop(ESP, isIndirect: true, size: RegisterSize.Long64);
            }
            else
            {
              XS.Pop(XSRegisters.EDX); // low part
              XS.Pop(XSRegisters.EAX); // high part
              XS.Add(ESP, EDX, destinationIsIndirect: true);
              XS.AddWithCarry(ESP, EAX, destinationDisplacement: 4);
            }
          }
          else
          {
            if (xIsFloat) //float
            {
              XS.SSE.MoveSS(XMM0, ESP, sourceIsIndirect: true);
              XS.Add(XSRegisters.ESP, 4);
              XS.SSE.MoveSS(XMM1, ESP, sourceIsIndirect: true);
              XS.SSE.AddSS(XMM0, XMM1);
              XS.SSE.MoveSS(XMM1, ESP, sourceIsIndirect: true);
            }
            else //integer
            {
              XS.Pop(XSRegisters.EAX);
              XS.Add(ESP, EAX, destinationIsIndirect: true);
            }
          }
        }
      }
    }
}
