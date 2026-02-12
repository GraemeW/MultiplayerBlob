# MultiplayerBlob

Multiplayer Blob is a game in which 'n' participants control a blob together through various adventures and shenanigans.

## Blob Construction

The blob is comprised of:
* some number of intrinsic agents, which give it its overall shape and some semblance of blob stability
* an additional number of extrinsic (i.e. player-controlled) agents, which dictate blob movement and actions

It follows that the extents of the blob are shaped by the movements of the intrinsic and extrinsic agents, which live as child game objects to the primary blob.  These child game objects are used to construct a mesh, which defines the outline of the blob, like below:

![](./Docs/BlobMovement.gif)

## Blob Movement and Agent Constraints

Each frame the blob checks for the average position of all extrinsic blob agents.  The blob will then attempt to move its rigidbody toward this position.

In order to prevent the blob from becoming too large, the blob has constraints on the distance that any given agent can move from its center point.  After moving toward the average agent position, the blob will then pull all agents outside sed constraints into range.
