using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Reflection;

public class CopiarComponentesEditor : EditorWindow
{
    #if UNITY_EDITOR
    private GameObject objetoOrigem; // Objeto de origem
    private GameObject objetoDestino; // Objeto de destino

    [MenuItem("Tools/Copiar Componentes")]
    public static void ShowWindow()
    {
        GetWindow<CopiarComponentesEditor>("Copiar Componentes");
    }

    void OnGUI()
    {
        GUILayout.Label("Copiar Componentes de um Objeto para Outro", EditorStyles.boldLabel);

        objetoOrigem = (GameObject)EditorGUILayout.ObjectField("Objeto Origem", objetoOrigem, typeof(GameObject), true);
        objetoDestino = (GameObject)EditorGUILayout.ObjectField("Objeto Destino", objetoDestino, typeof(GameObject), true);

        if (GUILayout.Button("Copiar Componentes"))
        {
            if (objetoOrigem != null && objetoDestino != null)
            {
                CopiarTodosComponentes(objetoOrigem, objetoDestino);
            }
            else
            {
                EditorUtility.DisplayDialog("Erro", "Por favor, selecione ambos os objetos!", "OK");
            }
        }
    }

    void CopiarTodosComponentes(GameObject origem, GameObject destino)
    {
        // Obtém todos os componentes no objeto de origem
        Component[] componentes = origem.GetComponents<Component>();

        foreach (Component comp in componentes)
        {
            // Não queremos copiar o Transform, pois ele já existe no objeto destino
            if (comp is Transform) continue;

            // Copia o componente para o objeto de destino
            System.Type tipoComponente = comp.GetType();
            Component novoComponente = destino.AddComponent(tipoComponente);

            // Copia os valores dos campos do componente original para o novo
            foreach (FieldInfo campo in tipoComponente.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try
                {
                    object valorCampo = campo.GetValue(comp);
                    campo.SetValue(novoComponente, valorCampo);
                }
                catch (System.Exception ex)
                {
                    Debug.LogWarning($"Não foi possível copiar o campo {campo.Name} do componente {tipoComponente.Name}: {ex.Message}");
                }
            }

            foreach (PropertyInfo propriedade in tipoComponente.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (propriedade.CanWrite)
                {
                    try
                    {
                        object valorPropriedade = propriedade.GetValue(comp, null);
                        propriedade.SetValue(novoComponente, valorPropriedade, null);
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogWarning($"Não foi possível copiar a propriedade {propriedade.Name} do componente {tipoComponente.Name}: {ex.Message}");
                    }
                }
            }
        }

        // Exibe uma mensagem de confirmação
        EditorUtility.DisplayDialog("Sucesso", "Componentes copiados com sucesso!", "OK");
    }
    #endif
}
