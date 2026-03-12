using UnityEngine;

/// <summary>
/// AbsFinder — busca "Absolute Radiance" y le adjunta el componente Abs.
///
/// Este componente es creado por AuraRadiance.OnSceneLoaded(GG_Radiance).
/// Los efectos de ESCENA (audiencia, zoom, colores) ya se aplicaron antes
/// de que este objeto exista. AbsFinder solo se encarga de esperar al boss.
/// </summary>
internal class AbsFinder : MonoBehaviour
{
    private GameObject _abs;
    private bool       _assigned;

    private void Start()
    {
        AuraRadiance.instance.Log("[AbsFinder] Activo — buscando Absolute Radiance...");
    }

    private void Update()
    {
        // Buscar el boss en cada frame hasta encontrarlo
        if (_abs == null)
        {
            _assigned = false;
            _abs = GameObject.Find("Absolute Radiance");
        }

        if (_assigned || _abs == null) return;

        _assigned = true;
        AuraRadiance.instance.Log("[AbsFinder] Absolute Radiance encontrada");

        var existing = _abs.GetComponent<Abs>();
        if (existing != null)
        {
            AuraRadiance.instance.Log("[AbsFinder] Abs ya existía destruyendo para reinicializar");
            Object.Destroy(existing);
        }


        _abs.AddComponent<Abs>();

        AuraRadiance.instance.Log("[AbsFinder] Abs añadido correctamente.");


        Destroy(this);
    }
}