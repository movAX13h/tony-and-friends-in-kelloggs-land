# Tony &amp; Friends in Kellogg's Land
Tools to dissect the 1994 DOS game produced by Factor 5 and Rauser Entertainment.

![image](https://user-images.githubusercontent.com/1974959/69583636-a2b13b80-0fdb-11ea-8d67-759b0f6520c1.png)

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
| .MAP | Game Map     | grid based level map |
| .BOB | Animated Sprite | data and x86 code to display; similar to BOB format of Turrican II |
| .PCC | Image        | PCX version 5, encoded, 8 bit per px |
| .ICO | Image        | |
| .TFX | TFMX Music   | |
| .SAM | TFMX Samples | |

## Notes

- START.EXE loads itself after loading DPMI16BIT.OVL (is a EXE) and RTM.EXE.
- Game looks for HISCORE.DAT at launch which does not exist (at least not in my copy of the game).
