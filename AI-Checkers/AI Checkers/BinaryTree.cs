using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AICheckers
{
    class BinaryTree<T>
    {
        private List<BinaryTree<T>> child;
        private T value;

        public BinaryTree(T value)
        {
            this.value = value;
            this.child = new List<BinaryTree<T>>();
        }

        public BinaryTree<T> Add(T value)
        {
            BinaryTree<T> child = new BinaryTree<T>(value);
            child.GetParent = this;
            this.child.Add(child);
            return child;
        }

        public void Traversal(Action<T> visitor)
        {
            this.traversal(visitor);
        }

        protected void traversal(Action<T> visitor)
        {
            visitor(this.value);
            foreach (BinaryTree<T> child in this.child)
                child.traversal(visitor);
        }

        public BinaryTree<T> GetParent
        {
            get;
            private set;
        }

        public List<BinaryTree<T>> GetChildren
        {
            get { return this.child; }
        }

        public T GetValue
        {
            get { return this.value; }
        }

    }
}
