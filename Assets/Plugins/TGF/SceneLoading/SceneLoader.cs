using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TGF.SceneLoading
{
	public class SceneLoader : MonoBehaviour
	{
		public static SceneLoader Instance;

		private void Awake()
		{
			Instance = this;
		}

		/// <summary>
		/// Loads the scene asynchronously with scene transition
		/// </summary>
		/// <param name="buildIndex"></param>
		public async Task LoadSceneAsync(int buildIndex)
		{
			AsyncOperation asyncSceneLoad = SceneManager.LoadSceneAsync(buildIndex);
			await LoadScene(asyncSceneLoad);
		}
		
		public async Task LoadSceneAsync(string sceneName)
		{
			AsyncOperation asyncSceneLoad = SceneManager.LoadSceneAsync(sceneName);
			await LoadScene(asyncSceneLoad);
		}

		private async Task LoadScene(AsyncOperation asyncOperation)
		{
			asyncOperation.allowSceneActivation = false;

			await SceneTransition.Instance.TransitOut();
			CleanUp();
			asyncOperation.allowSceneActivation = true;
			asyncOperation.completed += async delegate(AsyncOperation operation)
			{
				await SceneTransition.Instance.TransitIn();
			};
		}

		private static void CleanUp()
		{
			DOTween.KillAll();
		}
	}
}