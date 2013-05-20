#region License
/*
The MIT License (MIT)

Copyright (c) 2013 Textquell

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion

#region Abstract

#endregion

namespace Textquell.KiuBrick.Octrees
{
    using System;

    /// <summary>
    /// A generic octree class to store data in
    /// </summary>
    [Serializable]
    public class Octree<T> : IDisposable
    {
        /// <summary>
        /// stores a pointer to the root element of the Octree
        /// </summary>
        Node _root;

        public Octree()
        {
            _root = new Node();
        }

        public class Node : IDisposable
        {
            #region private Data fields
            /// <summary>
            /// valid mask tells whether each of the child slots actually contains a voxel
            /// </summary>
            byte _validmask;
            /// <summary>
            /// leaf mask further speciﬁes whether each of these voxels is a leaf
            /// </summary>
            byte _leafmask;
            /// <summary>
            /// stores a pointer to the next child of the parent node, thus aligning the memory 
            /// sequentially
            /// </summary>
            Node _neighbor;
            /// <summary>
            /// keeps a pointer to the first child. Pointing downwards the tree allows for arbitrar
            /// y root nodes and insertion at the top of the tree
            /// </summary>
            Node _firstChild;
            #endregion

            /// <summary>
            /// returns the number of leafs that this node stores in the smallest type possible
            /// </summary>
            public int LeafCount
            {
                get
                {
                    // found at http://graphics.stanford.edu/~seander/bithacks.html#CountBitsSetKernighan
                    int leafpositions = (_validmask & _leafmask); // the bitfield that know where a leaf node exists
                    int count;
                    for (count = 0; leafpositions > 0; count++)
                    {
                        leafpositions &= leafpositions - 1; // clear the least significant bit set
                    }
                    return count;
                }
            }
            /// <summary>
            /// Gets an array of int that contains position numbers of leafs. The values range is from 0-7
            /// </summary>
            public int[] LeafPositions
            {
                get
                {
                    int leafposition = _leafmask & _validmask;
                    int[] result = new int[LeafCount];
                    int arrayPosition = 0;

                    for (int i = 0; i < LeafCount; i++)
                    {
                        if ((leafposition & (1 << i)) != 0)
                        {
                            result[arrayPosition++] = i;
                        }
                    }
                    return result;
                }
            }

            #region IDisposable Member

            public void Dispose()
            {
                // TODO: go to every child and tell it to delete its data and children. Than delete
                // your own data
                throw new NotImplementedException();
            }

            #endregion
        }

        #region IDisposable Member

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}