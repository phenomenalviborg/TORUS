using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.XR;

namespace absurdjoy
{
    public class SpawnPlayerAvatar : SpawnPrefabOnConnect
    {
        public Transform localHeadReference;
        public Transform localRootReference;
        public OVRCustomSkeleton localLeftSkeleton;
        public OVRCustomSkeleton localRightSkeleton;

        
        
        protected override void RealtimeConnected(Realtime realtime)
        {
            
            ///Check if we have VR, if not spawn the tabletCam Avatar

            if (TabletCamPos.Inst.IsVREnabled())
            {
                var avatar = Realtime.Instantiate(prefab.name, ownedByClient, preventOwnershipTakeover, destroyWhenOwnerOrLastClientLeaves);
            
                // Connect the local control components with the avatar components (and only for our local avatar, not the remote avatars);
                avatar.GetComponent<PlayerAvatar>().LinkWithLocal(localHeadReference, localLeftSkeleton, localRightSkeleton, localRootReference);
            }
            else
            {
                var avatar = Realtime.Instantiate("TabletCamAvatar", ownedByClient, preventOwnershipTakeover, destroyWhenOwnerOrLastClientLeaves);
            }
            
            
            
        }
    }
}