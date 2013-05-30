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

namespace Textquell.KiuBrick.Graphics
{
    using System;

    /// <summary>
    /// A generic octree class to store data in
    /// </summary>
    [Serializable]
    public class Octree<T>: IDisposable
    {
        /// <summary>
        /// stores a pointer to the root element of the Octree
        /// </summary>
        OctreeNode _root;

        public Octree()
        {
            _root = new OctreeNode();
        }

        /// <summary>
        /// The node of an octree
        /// </summary>
        /// <remarks>
        /// A node is a data structure that is using masks and pointers to make data accessible. 
        /// It is storing tree information in two masks, the leaf mask and the valid mask. The 
        /// valid mask is able to tell where the current node has children. The leaf mask is able 
        /// to tell leaf nodes and branch nodes apart. They are 8 bit bitfields. Each bit position
        /// is a child position in the tree. When a bit is set in the bitfield, a child exists. So
        /// you have three situations to think of:
        /// 1: valid mask is zero, which means there are no children of this node
        /// 2: valid mask is non-zero but leaf mask is zero, which means that this node has no 
        /// leafs but branches
        /// 3: valid mask and leaf mask are both non-zero. At the position that both mask have a 
        /// bit set at, a leaf node exists. Set positions of only the valid mask are still branches
        /// </remarks>
        public class OctreeNode: IDisposable
        {
            // TODO: Think about multi-threading, most likely a node is accessible fron different threads. Maybe it should be made thread safe.
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
            OctreeNode _neighbor;
            /// <summary>
            /// keeps a pointer to the first child. Pointing downwards the tree allows for arbitrar
            /// y root nodes and insertion at the top of the tree
            /// </summary>
            OctreeNode _firstChild;
            /// <summary>
            /// is storing the data for each leaf node. This array is empty when there is no leaf
            /// node attached.
            /// </summary>
            T[] _data; // TODO: Is a List<T> fast enough for this?
            #endregion

            #region public Properties
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
                    for ( count = 0; leafpositions > 0; count++ )
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
                    int leafposition = _validmask & _leafmask;
                    int[] result = new int[LeafCount];
                    int arrayPosition = 0;

                    for ( int i = 0; i < LeafCount; i++ )
                    {
                        if ( (leafposition & (1 << i)) != 0 )
                        {
                            result[arrayPosition++] = i;
                        }
                    }
                    return result;
                }
            }
            /// <summary>
            /// finds out how many non-leaf children this node has
            /// </summary>
            public int BranchCount
            {
                get
                {
                    int branchpositions = (_validmask & ~(_leafmask));
                    int count;
                    for ( count = 0; branchpositions > 0; count++ )
                    {
                        branchpositions &= branchpositions - 1; // clear the least significant bit set
                    }
                    return count;
                }
            }
            /// <summary>
            /// finds out where non-leaf children are
            /// </summary>
            public int[] BranchPositions
            {
                get
                {
                    int branchposition = _validmask & _leafmask;
                    int[] result = new int[BranchCount];
                    int arrayPosition = 0;

                    for ( int i = 0; i < BranchCount; i++ )
                    {
                        if ( (branchposition & (1 << i)) != 0 )
                        {
                            result[arrayPosition++] = i;
                        }
                    }
                    return result;
                }
            }
            #endregion

            #region Data Getter and Setter
            public OctreeNode getNodeAtPosition( int position )
            {
                if ( position >= 8 || position <= -1 ) { throw new ArgumentException( "Position can only be between 0 and 7" ); }
                if ( BranchCount == 0 ) { throw new Exception( "There are no child nodes" ); }
                if ( (_validmask >> position) % 2 == 1 ) { } // OctreeNode exists because after shifting, every odd number indicates that the LSB is set

                throw new NotImplementedException();
            }

            public void insertNodeAtPosition( OctreeNode node, int position )
            {
                // TODO: Find out if the position is already occupied, if not, resize the _data 
                // array and arrange elements so their order is the same as the _validmask expects
                throw new NotImplementedException();
            }

            #endregion

            #region IDisposable Member

            public void Dispose()
            {
                foreach ( int position in BranchPositions )
                {
                    getNodeAtPosition( position ).Dispose();
                }
            }

            #endregion
        }

        #region IDisposable Member

        public void Dispose()
        {
            _root.Dispose();
        }

        #endregion
    }
}