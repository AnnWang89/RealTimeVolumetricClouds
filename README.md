# Clouds☁️: How to make volumetric cloud realistic and evolutional!
###### tags: `Computer Graphics`
## Proposal
* https://docs.google.com/presentation/d/1XdHw1atAELexWOSky_GNzWubzy7DHR1FumLBm8hRyXg/edit#slide=id.g19b628d4f30_2_50
* https://docs.google.com/document/d/1xoDwF73i6dEF6PIKQZIHhjMCxtX2-ozRIjy6tjBirhg/edit?usp=sharing

## 實作

### 說明
* 簡介
	![](https://i.imgur.com/47vTUYQ.png)


#### 介面: 
* 按下不同buttom會出現不同種雲，目前先假設有7種雲(下方有簡介)
* 在**CloudBox** 的 Inspector 中，有Cloud Setting 和 Noise 兩個可以調整係數的地方。藉由試著調整參數，來做出想要的雲的形狀。記好需要的數值，去程式碼裡改按下buttom後會變成的值。
	* **Clouds.cs**
		* Cloud Setting 的參數在 **Clouds.cs** 中 (注意!!不是Cloud Setting喔~)
		* 我沒有寫讓每一個都可以改，因為有一些好像不會動到(要加可以自己加~)。
			可以調整的為:
		```
		/* Coefficient */
			private float buttom_densityMultiplier;
			private float buttom_scale;
			private float buttom_densityOffset;
			private float buttom_volumeOffset;
			private float buttom_detailScale;
			private float buttom_detailMultiplier;
			private float buttom_heightMapFactor;

			private Int buttom_marchSteps;
			private float buttom_rayOffset;

			private float buttom_brightness;
			private float buttom_transmitThreshold;
			private float buttom_inScatterMultiplier;
			private float buttom_outScatterMultiplier;
		```
		* 直接在 對應的Buttom 的function裡面改係數就可以了!
			* 把 Settings.XXX 改成數字就ok~
		* 舉例:
		```
		private void Click_btnCirrocumulus()
		{
			Debug.Log("CLICK btnCirrocumulus IN!");
			choose_buttom = true;
			buttom_scale = 2;
			buttom_densityMultiplier = 10;
			buttom_densityOffset = Settings.densityOffset;
			buttom_volumeOffset = Settings.volumeOffset;
			buttom_detailScale = Settings.detailScale;
			buttom_detailMultiplier = Settings.detailMultiplier;

			buttom_heightMapFactor = Settings.heightMapFactor;

			buttom_marchSteps = Settings.marchSteps;
			buttom_rayOffset = Settings.rayOffset;

			buttom_brightness = (float)0.776;
			buttom_transmitThreshold = (float)0.48;
			buttom_inScatterMultiplier = (float)0.363;
			buttom_outScatterMultiplier = (float)0.517;

		}
		``` 
			
	* **Noise.cs**
		* 在 **Noise.cs** 中，一樣找到對應的Buttom 的function裡面改係數就可以了!
		* 簡單易懂，只要調整3個參數，分別代表noise的不同layer(簡介圖中右下部分):
			* private int choose_A;
    		* private int choose_B;
    		* private int choose_C;
    	* 舉例: 
    	```
		private void Click_btnCirrocumulus()
		{
			Debug.Log("CLICK btnCirrocumulus IN!");
			choose_buttom = true;
			choose_A = 40;
			choose_B = 43;
			choose_C = 45;

		}
		```
    	
	




## Reference
* YouTube:
    * https://www.youtube.com/watch?v=4QOcCGI6xOU&list=PLFt_AvWsXl0ehjAfLFsp1PGaatzAwo0uK&index=10
* Method:
    * **http://www.diva-portal.org/smash/get/diva2:1223894/FULLTEXT01.pdf (P.17 18 雲的種類)**
    	* Cumulus cloud ![](https://i.imgur.com/KNZxxJz.png)

		* Cumulonimbus cloud 
			* ![](https://i.imgur.com/sUvz4ce.png)

		* Stratus clouds 
			* ![](https://i.imgur.com/R17ZF8l.png)

		* Stratocumulus clouds
			* ![](https://i.imgur.com/5QLpkyl.png)

		* Altocumulus clouds
			* ![](https://i.imgur.com/jnzdQ6p.png)

		* Cirrus clouds 
			* ![](https://i.imgur.com/pzT6GPK.png)

		* Cirrocumulus clouds 
			* ![](https://i.imgur.com/wCxaAdF.png)
    * http://killzone.dl.playstation.net/killzone/horizonzerodawn/presentations/Siggraph15_Schneider_Real-Time_Volumetric_Cloudscapes_of_Horizon_Zero_Dawn.pdf (這個也好)
    * https://lup.lub.lu.se/luur/download?func=downloadFile&recordOId=8893256&fileOId=8893258 (參考用)
    * https://media.contentapi.ea.com/content/dam/eacom/frostbite/files/s2016-pbs-frostbite-sky-clouds-new.pdf (p.30~end)
* GitHub:
    * https://github.com/SebLague/Clouds 
    * https://github.com/adrianpolimeni/RealTimeVolumetricClouds (可從這個開始)
* 神奇的東西:
    * https://thebookofshaders.com/
    * http://magnuswrenninge.com/wp-content/uploads/2010/03/Wrenninge-OzTheGreatAndVolumetric.pdf
