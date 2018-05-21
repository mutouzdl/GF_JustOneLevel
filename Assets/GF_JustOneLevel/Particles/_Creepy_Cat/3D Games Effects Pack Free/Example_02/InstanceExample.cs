using UnityEngine;

/// ------------------------------
/// Creating instance of particles
/// ------------------------------
public class InstanceExample : MonoBehaviour
{
	/// ------------------------------
	/// Singleton
	/// ------------------------------
	public static InstanceExample Instance;

	public ParticleSystem effectA;
	public ParticleSystem effectB;

	void Awake()
	{
		/// ---------------------
		// Register the singleton
		/// ---------------------
		if (Instance != null)
		{
			Debug.LogError("Multiple instances of InstanceExample script!");
		}

		Instance = this;
	}

	void Update(){
		/// -----------------------------------------
		/// Instanciate into a box of 5 x 5 x 5 (xyz)
		/// -----------------------------------------
		InstanceExample.Instance.Explosion(new Vector3(Random.Range(-5.0f,5.0f),Random.Range(-5.0f,5.0f),Random.Range(-5.0f,5.0f)));
	}

	/// -----------------------------------------
	/// Create an explosion at the given location
	/// -----------------------------------------
	public void Explosion(Vector3 position)
	{
		instantiate(effectA, position);
		instantiate(effectB, position);
	}

	/// -----------------------------------------
	/// Instantiate a Particle system from prefab
	/// -----------------------------------------
	private ParticleSystem instantiate(ParticleSystem prefab, Vector3 position)
	{
		ParticleSystem newParticleSystem = Instantiate(prefab,position,Quaternion.identity) as ParticleSystem;

		/// -----------------------------
		// Make sure it will be destroyed
		/// -----------------------------
		Destroy(
			newParticleSystem.gameObject,
			newParticleSystem.startLifetime
		);

		return newParticleSystem;
	}



}