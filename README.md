# <img src="https://gitlab.com/Wacton/Arbortrary/raw/main/Arbortrary/Resources/Arbortrary.png" width="32" height="32"> Arbortrary
Arbortrary takes a seed and generates a unique random tree artwork (... arbortrarily?). Parameters can be adjusted to influence the result, but fundamental aspects such as relative node placement and colour modifications will remain the same.

For example, the default tree that represents the text "Arbortrary":<br>
`Wacton.Arbortrary.exe --text "Arbortrary"`
![Example: default tree for the text "Arbortrary"](Resources/example-1_default.png "Example: default tree for the text \"Arbortrary\"")

Specifying colours will determine initial colour values while the tree structure remains the same:<br>
`Wacton.Arbortrary.exe --text "Arbortrary" --background-colour "#404046" --first-node-colour "#E8E8FF"`
![Example: tree for the text "Arbortrary" with custom initial colours](Resources/example-2_initial-colours.png "Example: tree for the text \"Arbortrary\" with custom initial colours")

Other parameters include the number of nodes to generate:<br>
`Wacton.Arbortrary.exe --text "Arbortrary" --background-colour "#404046" --first-node-colour "#E8E8FF" --nodes 500`
![Example: tree for the text "Arbortrary" with custom initial colours and more nodes](Resources/example-3_initial-colours-more-nodes.png "Example: tree for the text \"Arbortrary\" with custom initial colours and more nodes")

Gif can also be generated:<br>
`Wacton.Arbortrary.exe --text "Arbortrary" --gif --background-colour "#E8E8FF" --first-node-colour "#404046" --width 960 --height 540 --zoom 0.5`
![Example: tree for the text "Arbortrary" as a gif with custom initial colours and scaled down](Resources/example-4_initial-colours-scaled.gif "Example: tree for the text \"Arbortrary\" as a gif with custom initial colours and scaled down")

<details>
<summary>Attributions 🙇</summary>

This application uses the following software packages in conformance to their respective licences:

- **CommandLineParser** ([MIT License](https://github.com/commandlineparser/commandline/blob/master/License.md) · [source](https://github.com/commandlineparser/commandline)),
  copyright © Giacomo Stelluti Scala & Contributors
- **SixLabors.ImageSharp** ([Apache License 2.0](https://github.com/SixLabors/ImageSharp/blob/master/LICENSE) · [source](https://github.com/SixLabors/ImageSharp)), copyright © Six Labors
- **SixLabors.ImageSharp.Drawing** ([Apache License 2.0](https://github.com/SixLabors/ImageSharp.Drawing/blob/master/LICENSE) · [source](https://github.com/SixLabors/ImageSharp.Drawing)), copyright © Six Labors
- **Wacton.Unicolour** ([MIT License](https://gitlab.com/Wacton/Unicolour/-/blob/main/LICENSE) · [source](https://gitlab.com/Wacton/Unicolour)),
  copyright © William Acton

</details>

---

[Wacton.Arbortrary](https://gitlab.com/Wacton/Arbortrary) is licensed under the [GNU General Public License v3.0](https://gitlab.com/Wacton/Arbortrary/-/blob/main/LICENSE), copyright © 2021 William Acton.
