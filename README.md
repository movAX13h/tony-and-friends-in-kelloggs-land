# Tony &amp; Friends in Kellogg's Land
Tools to dissect the 1994 DOS game produced by Factor 5.


## Game files

| Name | Details |
|:-----|:------|
| START.EXE    | main game binary                             |
| SETUP.EXE    | game setup program                           |
| KELLOGG.INI  | sound card setup data (written by SETUP.EXE) |
| PCKELL.DAT   | Assets container file (see list below)       |
| DPMI16BI.OVL | Borland 16-bit DOS Protected Mode Interface  |
| RTM.EXE      | Borland 16-bit DOS Protected Mode Interface  |


## Asset files

These file types are contained in PCKELL.DAT:

| Ext. | Name | Details |
|:----:|:-----|:--------|
| .ARE | Area         | |
| .BOB | Animated Sprite | data and x86 code to display; similar to BOB format of Turrican II |
| .ICO | Image        | |
| .MAP | Game Map     | grid based level map |
| .PCC | Image        | PC Paintbrush File Format a.k.a. PCX |
| .TFX | TFMX Music   | |
| .SAM | TFMX Samples | |

## Notes

- START.EXE loads itself after loading DPMI16BIT.OVL (is a EXE) and RTM.EXE.
- Game looks for HISCORE.DAT at launch which does not exist (at least not in my copy of the game).
