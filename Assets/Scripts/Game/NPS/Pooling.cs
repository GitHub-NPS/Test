using System.Collections.Generic;
using UnityEngine;

namespace NPS
{
    [System.Serializable]
    public class Pooling<T> where T : MonoBehaviour
    {
        /* Note */
        // Sử dụng theo bộ khi pooling object:
        // Bộ 1 - Không truyền tham số:  Release() + Get()
        // Bộ 2 - Có truyền tham số:  Release(from) + Get(idx, true) 

        // Bộ 2 => Tối ưu hơn nhưng cần xác định rõ from: kích thức mảng mới sẽ tạo

        /// <summary>
        /// Kiểm tra prefab có NULL không
        /// </summary>
        public bool IsValid { get => this.prefab; }
        /// <summary>
        /// Kích thước mảng In
        /// </summary>
        public int Count { get => _inUse.Count; }

        /// <summary>
        /// Mảng Un - Chưa sử dụng
        /// </summary>
        [SerializeField] public List<T> _unUse = new List<T>();
        /// <summary>
        /// Mảng In - Đang sử dụng
        /// </summary>
        [SerializeField] public List<T> _inUse = new List<T>();

        /// <summary>
        /// Prefab sẽ Pooling
        /// </summary>
        [SerializeField] T prefab;
        /// <summary>
        /// Parent chứa object
        /// </summary>
        [SerializeField] public Transform parent;

        public Pooling()
        {

        }

        /// <summary>
        /// Set lại prefab và parent khi thay đổi hoặc !IsValid
        /// </summary>
        public void Set(T prefab, Transform parent = null)
        {
            this.prefab = prefab;
            if (parent) this.parent = parent;
        }

        /// <summary>
        /// Lấy phần tử đầu mảng Un ra sử dụng, Nếu chưa có tự tạo thêm
        /// </summary>
        public T Get()
        {
            if (IsValid)
            {
                lock (_unUse)
                {
                    if (_unUse.Count != 0)
                    {
                        T po = _unUse[0];
                        _inUse.Add(po);
                        _unUse.RemoveAt(0);
                        po.gameObject.SetActive(true);
                        return po;
                    }
                    else
                    {
                        T po = parent ? MonoBehaviour.Instantiate(prefab, parent) : MonoBehaviour.Instantiate(prefab);
                        _inUse.Add(po);
                        return po;
                    }
                }
            }
            else
            {
                Debug.LogError("Pooling Object: Prefab NULL");
                return null;
            }
        }

        /// <summary>
        /// Lấy phần tử thứ idx trong mảng In, Nếu chưa có thì tạo thêm (khi isCreate = true)
        /// </summary>
        /// <param name="idx">Thứ tự trong mảng In</param>
        /// <param name="isCreate">Có tạo thêm khi null hay không</param>
        public T Get(int idx, int from = -1)
        {
            if (((idx < from && from != -1) || from == -1) && idx < Count)
            {
                return _inUse[idx];
            }
            else
            {
                if (from != -1)
                {
                    return Get();
                }
                else
                {
                    Debug.LogError("Pooling Object: Prefab NULL: " + idx);
                    return null;
                }
            }
        }

        /// <summary>
        /// Giải phóng object chỉ định
        /// </summary>
        public void Release(T po)
        {
            po.gameObject.SetActive(false);

            lock (_unUse)
            {
                _unUse.Add(po);
                _inUse.Remove(po);
            }
        }

        /// <summary>
        /// Giải phóng mảng In từ from -> hết
        /// </summary>
        /// <param name="from">Vị trí bắt đầu</param>
        public void Release(int from = 0)
        {
            for (int i = from; i < _inUse.Count; i++)
            {
                _inUse[i].gameObject.SetActive(false);
                lock (_unUse)
                {
                    _unUse.Insert(0, _inUse[i]);
                }
            }
            if (from > Count) _inUse.Clear();
            else _inUse.RemoveRange(from, Count - from);
        }

        /// <summary>
        /// Hủy 2 mảng In và Un
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < _unUse.Count; i++)
            {
                MonoBehaviour.Destroy(_unUse[i].gameObject);
            }
            for (int i = 0; i < _inUse.Count; i++)
            {
                MonoBehaviour.Destroy(_inUse[i].gameObject);
            }
            _unUse.Clear();
            _inUse.Clear();
        }
    }
}
