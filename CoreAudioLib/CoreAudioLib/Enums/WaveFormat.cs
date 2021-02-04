// *****************************************************************************************************
//   Copyright (c) 2019, HP Development Company, L.P. All rights reserved.
//   This software contains confidential and proprietary information of HP.
//   The user of this software agrees not to disclose, disseminate or copy
//   such Confidential Information and shall use the software only in accordance
//   with the terms of the license agreement the user entered into with HP.
// 
//   WaveFormat.cs
// 
// 
//   Description:
//      The enum Wave Format
//
// *****************************************************************************************************
using System;

namespace CoreAudioLib.Enums
{
    /// <summary>
    /// The enum Wave Format
    /// </summary>
    enum WaveFormat : UInt16
    {
        /// <summary>WAVE_FORMAT_UNKNOWN,	Microsoft Corporation</summary>
        Unknown = 0x0000,

        /// <summary>WAVE_FORMAT_PCM		Microsoft Corporation</summary>
        Pcm = 0x0001,

        /// <summary>WAVE_FORMAT_ADPCM		Microsoft Corporation</summary>
        Adpcm = 0x0002,

        /// <summary>WAVE_FORMAT_IEEE_FLOAT Microsoft Corporation</summary>
        IeeeFloat = 0x0003,

        /// <summary>WAVE_FORMAT_ALAW		Microsoft Corporation</summary>
        ALaw = 0x0006,

        /// <summary>WAVE_FORMAT_MULAW		Microsoft Corporation</summary>
        MuLaw = 0x0007,

        /// <summary>WAVE_FORMAT_DTS		Microsoft Corporation</summary>
        Dts = 0x0008,

        /// <summary>WAVE_FORMAT_DRM		Microsoft Corporation</summary>
        Drm = 0x0009,
       
        /// <summary>WAVE_FORMAT_GSM610, Microsoft Corporation</summary>
        Gsm610 = 0x0031,

        /// <summary>WAVE_FORMAT_MSNAUDIO, Microsoft Corporation</summary>
        MsnAudio = 0x0032,

        /// <summary></summary>
        WAVE_FORMAT_MSG723 = 0x0042, // Microsoft Corporation 
     
        /// <summary></summary>
        WAVE_FORMAT_DSAT_DISPLAY = 0x0067, // Microsoft Corporation 
       
        /// <summary></summary>
        WAVE_FORMAT_MSAUDIO1 = 0x0160, // Microsoft Corporation
        /// <summary>
        /// Windows Media Audio, WAVE_FORMAT_WMAUDIO2, Microsoft Corporation
        /// </summary>
        WindowsMediaAudio = 0x0161,

        /// <summary>
        /// Windows Media Audio Professional WAVE_FORMAT_WMAUDIO3, Microsoft Corporation
        /// </summary>
        WindowsMediaAudioProfessional = 0x0162,

        /// <summary>
        /// Windows Media Audio Lossless, WAVE_FORMAT_WMAUDIO_LOSSLESS
        /// </summary>
        WindowsMediaAudioLosseless = 0x0163,

        /// <summary>
        /// Advanced Audio Coding (AAC) audio in Audio Data Transport Stream (ADTS) format.
        /// The format block is a WAVEFORMATEX structure with wFormatTag equal to WAVE_FORMAT_MPEG_ADTS_AAC.
        /// </summary>
        /// <remarks>
        /// The WAVEFORMATEX structure specifies the core AAC-LC sample rate and number of channels, 
        /// prior to applying spectral band replication (SBR) or parametric stereo (PS) tools, if present.
        /// No additional data is required after the WAVEFORMATEX structure.
        /// </remarks>
        /// <see>http://msdn.microsoft.com/en-us/library/dd317599%28VS.85%29.aspx</see>
        MPEG_ADTS_AAC = 0x1600,

        /// <summary>
        /// MPEG-4 audio transport stream with a synchronization layer (LOAS) and a multiplex layer (LATM).
        /// The format block is a WAVEFORMATEX structure with wFormatTag equal to WAVE_FORMAT_MPEG_LOAS.
        /// </summary>
        /// <remarks>
        /// The WAVEFORMATEX structure specifies the core AAC-LC sample rate and number of channels, 
        /// prior to applying spectral SBR or PS tools, if present.
        /// No additional data is required after the WAVEFORMATEX structure.
        /// </remarks>
        /// <see>http://msdn.microsoft.com/en-us/library/dd317599%28VS.85%29.aspx</see>
        MPEG_LOAS = 0x1602,

        /// <summary>
        /// High-Efficiency Advanced Audio Coding (HE-AAC) stream.
        /// The format block is an HEAACWAVEFORMAT structure.
        /// </summary>
        /// <see>http://msdn.microsoft.com/en-us/library/dd317599%28VS.85%29.aspx</see>
        MPEG_HEAAC = 0x1610,

        /// <summary>WAVE_FORMAT_EXTENSIBLE</summary>
        Extensible = 0xFFFE, // Microsoft 
        /// <summary></summary>
        WAVE_FORMAT_DEVELOPMENT = 0xFFFF,
    }
}
