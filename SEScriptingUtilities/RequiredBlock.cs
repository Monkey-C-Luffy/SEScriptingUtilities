/*
 MIT License

Copyright (c) 2022 Monkey C Luffy

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using System;
using Sandbox.ModAPI.Ingame;

namespace SEScripting
{
    public class RequiredBlock : IEquatable<RequiredBlock>
    {
        private IMyTerminalBlock _block = null;
        public IMyTerminalBlock Block
        {
            get
            {
                if(_block == null)
                {
                    LoadBlock();
                }
                return _block;
            }
            private set
            {
                _block = value;
            }
        }
        public Type BlockType { get; private set; }
        public string Name { get; private set; }
        public string Identifier { get; private set; }

        public bool Exists { get; private set; }

        public RequiredBlock(string blockIdentifier)
        {
            Identifier = blockIdentifier;
            BlockType = null;
            Name = "";
            Exists = false;
        }

        public IMyTerminalBlock LoadBlock()
        {
            if(CheckBlockExists())
            {
                Block = Utils.GetRequiredBlockByKey(Identifier);
                if(Block != null)
                {
                    Name = Block.DisplayNameText;
                    BlockType = Block.GetType();
                    Exists = true;
                    return Block;
                }
            }
            DebugBlockFound();
            return null;
        }

        public bool CheckBlockExists()
        {
            Exists = Utils.FindRequiredBlocksByKey(Identifier);
            DebugBlockFound();
            return Exists;
        }
        public T GetBlock<T>() where T : class
        {
            return Block as T;
        }

        private void DebugBlockFound()
        {
            if(!BlocksManager.DebugEnable) return;
            Utils.FoundBlock(Exists,Identifier,Block);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() + Identifier.GetHashCode() + Block.GetHashCode() + BlockType.GetHashCode();
        }

        public bool Equals(RequiredBlock other)
        {
            return Block == other.Block && Identifier == other.Identifier && BlockType == other.BlockType;
        }
    }
}
