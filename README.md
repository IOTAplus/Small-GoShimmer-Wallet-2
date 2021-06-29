# Small-GoShimmer-Wallet-2

Welcome to the IOTA+ Small GoShimmer Wallet 2
You will get here some informations how to use this Github files.

## How to use the Goshimmer Wallet with the Windows executable (.exe)

Download the the complete Github, export it and open the "Small-GoShimmer_Wallet-2-Compiled" folder. 
Open the Windows folder and then the folder with the last version.
There you should be able to find the "Small-GoShimmer-Wallet-2.exe" file. 
Double click it to start the "Small GoShimmer Wallet 2".
At the first start you will notice that it will generate new files "config.json", "wallet.dat" and "wallet.dat.bgp" it that folder.
There you can find some additional information about the configuration like the connected node, reusable address and generated seed.
Check if all of these 3 files where generated and keep reeding the trouble shoot part if not.

## How to use the Goshimmer Wallet with the Windows executable (.exe)

When you start the first time the "Small-GoShimmer-Wallet-2.exe" you will see an empty wallet without any balance. 
Click on the "Request Funds" button and then on "Balance" to update the balances.

## Trouble Shooting
If for the files "wallet.dat" and "wallet.dat.bgp" are not generated, probably the connection to the noded failed and you can manually copy and paste them from the "Wallet-Files-If-Connection-Fails" folder to the same folder where the "config.json" and the "Small-GoShimmer-Wallet-2.exe" file are. Please be aware that these contains the SEED. So you would share this wallet with others having the same problem. This solution would not fix the conncection issue completely. The app coould still have problems to run smoothely.

You can later just delete the files and restart the wallet again. The files should be generated as soon as the the connection to the server issue isn't anymore.
Check the conncection again by contorlling if the files are generated automaticall by just restarting the wallet after deleting the "wallet.dat" and "wallet.dat.bgp" files.
