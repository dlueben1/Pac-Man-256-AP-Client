using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApPac256.Data
{
    public static class PowerUpData
    {
        public static Dictionary<string, PowerupType> PowerUpNameMap = new Dictionary<string, PowerupType>
        {
            {  "Progressive Freeze", PowerupType.Freeze },
            {  "Progressive Laser", PowerupType.Laser },
            {  "Progressive Bomb", PowerupType.Bomb },
            {  "Progressive Giant", PowerupType.Giant },
            {  "Progressive Tornado", PowerupType.Tornado },
            {  "Progressive Stealth", PowerupType.Stealth },
            {  "Progressive Fire", PowerupType.Fire },
            {  "Progressive Trap", PowerupType.Trap },
            {  "Progressive Magnet", PowerupType.Magnet },
            {  "Progressive Pac-Men", PowerupType.PacMen },
            {  "Progressive Radar", PowerupType.Radar },
            {  "Progressive Shatter", PowerupType.Shatter },
            {  "Progressive Twinado", PowerupType.Twinado },
            {  "Progressive Pyro", PowerupType.Pyro },
            {  "Progressive Optics", PowerupType.Optics },
            {  "Progressive Boom", PowerupType.Boom },
            {  "Progressive Regen", PowerupType.Regen },
            {  "Progressive Sonar", PowerupType.Sonar },
            {  "Progressive Beam", PowerupType.Beam },
            {  "Progressive Cherries", PowerupType.Cherries },
            {  "Progressive Electric", PowerupType.Electric },
        };
    }
}
