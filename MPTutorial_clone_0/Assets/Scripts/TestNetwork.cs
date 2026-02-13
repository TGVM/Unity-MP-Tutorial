using PurrNet;
using UnityEngine;

public class TestNetwork : NetworkIdentity
{
    [SerializeField] private Color _color;
    [SerializeField] private Renderer _renderer;

    [SerializeField] private SyncVar<int> _health = new(initialValue: 100);

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            var myStruct = new TestStruct()
            {
                color = _color
            };
            SetColor(myStruct);
        }
        if (Input.GetKey(KeyCode.S))
        {
            TakeDamage(10);
        }
    }

    [ObserversRpc(bufferLast:true)]
    private void SetColor(TestStruct myStruct)
    {
        _renderer.material.color = myStruct.color;
    }

    [ServerRpc]
    private void TakeDamage(int damage)
    {
        _health.value -= damage;
    }

    private struct TestStruct
    {
        public Color color;
    }
}
