using GameManager;
using UnityEngine;

namespace Room
{
    public class RoomManager : MonoBehaviour
    {
        public GameObject curRoom;
        public Transform player;
        public GameDataManager gdm;
        /**
         * 0 for left
         * 1 for right
         * 2 for up
         * 3 for bottom
         */
        private readonly Vector3[] _arr = new Vector3[4]
        {
            new Vector3(-50, 0, 0),
            new Vector3(50, 0, 0),
            new Vector3(0, 0, 50),
            new Vector3(0, 0, -50)
        };
        private readonly GameObject[] _existenceRoom = new GameObject[4];

        
        public void Init(GameDataManager gm)
        {
            gdm = gm;
            NewRoom();
        }

        private void Update()
        {
            var defDist = Vector3.Distance(player.position, transform.position);
            for (var i = 0; i < 4; i++)
            {
                if (!(defDist > Vector3.Distance(player.position, transform.position + _arr[i]))) continue;
                Destroy(_existenceRoom[i ^ 1]);
                Destroy(_existenceRoom[i ^ 2]);
                Destroy(_existenceRoom[i ^ 3]);

                curRoom.GetComponent<Room>().Exit();
                _existenceRoom[i].GetComponent<Room>().Enter();

                _existenceRoom[i ^ 1] = curRoom;
                curRoom = _existenceRoom[i];
                _existenceRoom[i] = null;
                _existenceRoom[i ^ 2] = null;
                _existenceRoom[i ^ 3] = null;
                transform.position += _arr[i];
                NewRoom();
                return;
            }
        }


        private void NewRoom()
        {
            for (var i = 0; i < 4; i++)
            {
                if (_existenceRoom[i] == null)
                {
                    _existenceRoom[i] = Instantiate(gdm.GetRandomRoom().roomPrefab,
                        curRoom.transform.position + _arr[i], curRoom.transform.rotation);
                }
            }
        }
    }
}