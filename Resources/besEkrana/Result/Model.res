MWS Result File Version 20150206
size=i:27

type=s:HIDDENITEM
problemclass=s::8:1000
visibility=s:hidden
creation=s:internal
lifetime=s:surviveparchange
result=s:1
files=s:simulation_overview.json

type=s:HIDDENITEM
problemclass=s::8:1000
visibility=s:hidden
creation=s:internal
lifetime=s:rebuild
result=s:1
files=s:RefSpectrum_pw.sig

type=s:HIDDENITEM
problemclass=s::8:1000
visibility=s:hidden
creation=s:internal
lifetime=s:rebuild
result=s:1
files=s:e-field (f=2)_pw.m3d

type=s:HIDDENITEM
problemclass=s::8:1000
visibility=s:hidden
creation=s:internal
lifetime=s:rebuild
result=s:1
files=s:h-field (f=2)_pw.m3d

type=s:HIDDENITEM
problemclass=s::8:1000
visibility=s:hidden
creation=s:internal
lifetime=s:solverstart
result=s:0
files=s:PBAMeshDetails.axg

type=s:HIDDENITEM
problemclass=s::8:1000
visibility=s:hidden
creation=s:internal
lifetime=s:solverstart
result=s:0
files=s:PBAConnectivity.axg

type=s:HIDDENITEM
problemclass=s::8:1000
visibility=s:hidden
creation=s:internal
lifetime=s:solverstart
result=s:0
files=s:PBAMeshFeedback.axg

type=s:HIDDENITEM
problemclass=s::8:1000
visibility=s:hidden
creation=s:internal
lifetime=s:rebuild
result=s:1
files=s:World.fid

type=s:HIDDENITEM
problemclass=s::8:1000
visibility=s:hidden
creation=s:internal
lifetime=s:survivemeshadapt
result=s:1
files=s:model.gex

type=s:HIDDENITEM
problemclass=s::8:1000
visibility=s:hidden
creation=s:internal
lifetime=s:survivemeshadapt
result=s:1
files=s:PP.sid

type=s:HIDDENITEM
problemclass=s::8:1000
visibility=s:hidden
creation=s:internal
lifetime=s:survivemeshadapt
result=s:1
files=s:PP.fmm

type=s:FOLDER
problemclass=s::8:1000
visibility=s:visible
creation=s:internal
lifetime=s:persistent
result=s:0
treepath=s:1D Results

type=s:XYSIGNAL
subtype=s:user
problemclass=s::4:3
visibility=s:visible
creation=s:internal
lifetime=s:persistent
result=s:0
treepath=s:Excitation Signals\default
files=s:signal_default_lf.sig

type=s:XYSIGNAL
subtype=s:user
problemclass=s::0:0
visibility=s:visible
creation=s:internal
lifetime=s:persistent
result=s:0
treepath=s:Excitation Signals\default
files=s:signal_default.sig

type=s:MESH_FEEDBACK
problemclass=s::8:1000
visibility=s:visible
creation=s:internal
lifetime=s:solverstart
result=s:0
treepath=s:Mesh\Information\PBA
files=s:PBAMeshFeedback.rex
ylabel=s:Mesh Feedback

type=s:MESH_FEEDBACK
problemclass=s::8:1000
visibility=s:visible
creation=s:internal
lifetime=s:solverstart
result=s:0
treepath=s:Mesh\Information\Connectivity
files=s:PBAConnectivity.rex
ylabel=s:Mesh Feedback

type=s:MESH_FEEDBACK
problemclass=s::8:1000
visibility=s:visible
creation=s:internal
lifetime=s:solverstart
result=s:0
treepath=s:Mesh\Information\PBADetails
files=s:PBAMeshDetails.rex
ylabel=s:Mesh Feedback

type=s:XYSIGNAL
subtype=s:user
problemclass=s::8:1000
visibility=s:visible
creation=s:internal
lifetime=s:rebuild
result=s:1
treepath=s:1D Results\Port signals\Plane wave
files=s:plw.sig

type=s:XYSIGNAL
subtype=s:user
problemclass=s::8:1000
visibility=s:visible
creation=s:internal
lifetime=s:rebuild
result=s:1
treepath=s:1D Results\Probes\H-Field\Probe Signals\H-Field (Y; 25 -10 25) [pw]
files=s:H-Field (Y; 25 -10 25)(pw).prs

type=s:XYSIGNAL
subtype=s:complex
problemclass=s::8:1000
visibility=s:visible
creation=s:internal
lifetime=s:rebuild
result=s:1
treepath=s:1D Results\Probes\H-Field\H-Field (Y; 25 -10 25) [pw]
files=s:H-Field (Y; 25 -10 25)(pw).prc

type=s:XYSIGNAL
subtype=s:user
problemclass=s::8:1000
visibility=s:visible
creation=s:internal
lifetime=s:rebuild
result=s:1
treepath=s:1D Results\Probes\E-Field\Probe Signals\E-Field (Y; 25 -10 25) [pw]
files=s:E-Field (Y; 25 -10 25)(pw).prs

type=s:XYSIGNAL
subtype=s:complex
problemclass=s::8:1000
visibility=s:visible
creation=s:internal
lifetime=s:rebuild
result=s:1
treepath=s:1D Results\Probes\E-Field\E-Field (Y; 25 -10 25) [pw]
files=s:E-Field (Y; 25 -10 25)(pw).prc

type=s:HFIELD3D
problemclass=s::8:1000
visibility=s:visible
creation=s:internal
lifetime=s:rebuild
result=s:1
treepath=s:2D/3D Results\H-Field\h-field (f=2) [pw]
files=s:h-field (f=2)_pw.m3d
files=s:h-field (f=2)_pw_m3d.rex

type=s:SURFACECURRENT
problemclass=s::8:1000
visibility=s:visible
creation=s:internal
lifetime=s:rebuild
result=s:1
treepath=s:2D/3D Results\Surface Current\surface current (f=2) [pw]
files=s:h-field (f=2)_pw.m3d
files=s:h-field (f=2)_pw_m3d_sct.rex

type=s:EFIELD3D
problemclass=s::8:1000
visibility=s:visible
creation=s:internal
lifetime=s:rebuild
result=s:1
treepath=s:2D/3D Results\E-Field\e-field (f=2) [pw]
files=s:e-field (f=2)_pw.m3d
files=s:e-field (f=2)_pw_m3d.rex

type=s:XYSIGNAL
subtype=s:energy
problemclass=s::8:1000
visibility=s:visible
creation=s:internal
lifetime=s:rebuild
result=s:1
treepath=s:1D Results\Energy\Energy [pw]
files=s:pw.eng

type=s:RESULT_0D
problemclass=s::8:1000
visibility=s:hidden
creation=s:internal
lifetime=s:rebuild
result=s:1
treepath=s:1D Results\AutomaticRunInformation
files=s:AutomaticRunInformation

