# <img src="https://gitlab.com/Wacton/Arbortrary/raw/main/Arbortrary/Resources/Arbortrary.png" width="32" height="32"> Arbortrary
Arbortrary takes a seed and generates a unique random tree artwork (... arbortrarily?). Parameters can be adjusted to influence the result, but fundamental aspects such as relative node placement and colour modifications will remain the same.

For example, the default tree that represents the text "Arbortrary":<br>
`Wacton.Arbortrary.exe --text "Arbortrary"`
![Example: default tree for the text "Arbortrary"](Resources/example-1_default.png "Example: default tree for the text \"Arbortrary\"")

Specifying colours will determine initial colours while the tree structure remains the same:<br>
`Wacton.Arbortrary.exe --text "Arbortrary" --background-colour "#404040" --first-node-colour "#E8E8E8"`
![Example: tree for the text "Arbortrary" with custom initial colours](Resources/example-2_initial-colours.png "Example: tree for the text \"Arbortrary\" with custom initial colours")

Other parameters include the number of nodes to generate:<br>
`Wacton.Arbortrary.exe --text "Arbortrary" --background-colour "#404040" --first-node-colour "#E8E8E8" --nodes 500`
![Example: tree for the text "Arbortrary" with custom initial colours and more nodes](Resources/example-3_initial-colours-more-nodes.png "Example: tree for the text \"Arbortrary\" with custom initial colours and more nodes")
