using PurrNet;
using TMPro;
using UnityEngine;

public class TestNetwork : NetworkIdentity
{
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private Color _color;
    [SerializeField] private Renderer _renderer;

    [SerializeField] private SyncVar<int> _health = new(initialValue: 100);

    private void Awake()
    {
        _health.onChanged += OnChanged;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _health.onChanged -= OnChanged;
    }

    private void OnChanged(int newValue)
    {
        _healthText.text = newValue.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var myStruct = new TestStruct()
            {
                color = _color
            };
            SetColor(myStruct);
        }
        if (Input.GetKeyDown(KeyCode.S))
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

        if(_health.value <= 0)
        {
            _health.value = 0;
            Debug.Log($"has died!");
        }
    }

    private struct TestStruct
    {
        public Color color;
    }
}
