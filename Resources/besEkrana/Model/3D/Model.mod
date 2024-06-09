'# MWS Version: Version 2024.1 - Oct 16 2023 - ACIS 33.0.1 -

'# length = mm
'# frequency = GHz
'# time = ns
'# frequency range: fmin = 1 fmax = 3
'# created = '[VERSION]2024.1|33.0.1|20231016[/VERSION]


'@ define material: Air

'[VERSION]2024.1|33.0.1|20231016[/VERSION]
With Material
     .Reset
     .Name "Air"
     .Folder ""
     .FrqType "all"
     .Type "Normal"
     .SetMaterialUnit "Hz", "mm"
     .Epsilon "1.00059"
     .Mu "1.0"
     .Kappa "0"
     .TanD "0.0"
     .TanDFreq "0.0"
     .TanDGiven "False"
     .TanDModel "ConstKappa"
     .KappaM "0"
     .TanDM "0.0"
     .TanDMFreq "0.0"
     .TanDMGiven "False"
     .TanDMModel "ConstKappa"
     .DispModelEps "None"
     .DispModelMu "None"
     .DispersiveFittingSchemeEps "General 1st"
     .DispersiveFittingSchemeMu "General 1st"
     .UseGeneralDispersionEps "False"
     .UseGeneralDispersionMu "False"
     .Rho "1.204"
     .ThermalType "Normal"
     .ThermalConductivity "0.026"
     .SpecificHeat "1005", "J/K/kg"
     .SetActiveMaterial "all"
     .Colour "0.682353", "0.717647", "1"
     .Wireframe "False"
     .Transparency "0"
     .Create
End With

'@ new component: component1

'[VERSION]2024.1|33.0.1|20231016[/VERSION]
Component.New "component1"

'@ define brick: component1:solid1

'[VERSION]2024.1|33.0.1|20231016[/VERSION]
With Brick
     .Reset 
     .Name "solid1" 
     .Component "component1" 
     .Material "Air" 
     .Xrange "0", "50" 
     .Yrange "0", "5" 
     .Zrange "0", "50" 
     .Create
End With

'@ define frequency range

'[VERSION]2024.1|33.0.1|20231016[/VERSION]
Solver.FrequencyRange "1", "3"

'@ define monitor: e-field (f=2)

'[VERSION]2024.1|33.0.1|20231016[/VERSION]
With Monitor 
     .Reset 
     .Name "e-field (f=2)" 
     .Dimension "Volume" 
     .Domain "Frequency" 
     .FieldType "Efield" 
     .MonitorValue "2" 
     .UseSubvolume "False" 
     .Coordinates "Structure" 
     .SetSubvolume "0", "50", "0", "5", "0", "50" 
     .SetSubvolumeOffset "0.0", "0.0", "0.0", "0.0", "0.0", "0.0" 
     .SetSubvolumeInflateWithOffset "False" 
     .Create 
End With

'@ define monitor: h-field (f=2)

'[VERSION]2024.1|33.0.1|20231016[/VERSION]
With Monitor 
     .Reset 
     .Name "h-field (f=2)" 
     .Dimension "Volume" 
     .Domain "Frequency" 
     .FieldType "Hfield" 
     .MonitorValue "2" 
     .UseSubvolume "False" 
     .Coordinates "Structure" 
     .SetSubvolume "0", "50", "0", "5", "0", "50" 
     .SetSubvolumeOffset "0.0", "0.0", "0.0", "0.0", "0.0", "0.0" 
     .SetSubvolumeInflateWithOffset "False" 
     .Create 
End With

'@ define probe: 0

'[VERSION]2024.1|33.0.1|20231016[/VERSION]
With Probe
     .Reset 
     .ID "0" 
     .AutoLabel "True" 
     .Field "Efield" 
     .Orientation "All" 
     .Xpos "5" 
     .Ypos "-3" 
     .Zpos "3" 
     .Create
End With

'@ modify probe: 0

'[VERSION]2024.1|33.0.1|20231016[/VERSION]
Probe.DeleteById "0" 

With Probe
     .Reset 
     .ID "0" 
     .AutoLabel "True" 
     .Field "Efield" 
     .Orientation "Y" 
     .Xpos "25" 
     .Ypos "-10" 
     .Zpos "25" 
     .Create
End With

'@ define probe: 1

'[VERSION]2024.1|33.0.1|20231016[/VERSION]
With Probe
     .Reset 
     .ID "1" 
     .AutoLabel "True" 
     .Field "Hfield" 
     .Orientation "Y" 
     .Xpos "25" 
     .Ypos "-10" 
     .Zpos "25" 
     .Create
End With

'@ define plane wave properties

'[VERSION]2024.1|33.0.1|20231016[/VERSION]
With PlaneWave
     .Reset 
     .Normal "0", "-1", "0" 
     .EVector "0", "0", "100" 
     .Polarization "Linear" 
     .ReferenceFrequency "2" 
     .PhaseDifference "-90.0" 
     .CircularDirection "Left" 
     .AxialRatio "0.0" 
     .SetUserDecouplingPlane "False" 
     .Store
End With

'@ define time domain solver parameters

'[VERSION]2024.1|33.0.1|20231016[/VERSION]
Mesh.SetCreator "High Frequency" 

With Solver 
     .Method "Hexahedral"
     .CalculationType "TD-PLW"
     .StimulationPort "Plane wave"
     .StimulationMode "1"
     .SteadyStateLimit "-40"
     .MeshAdaption "False"
     .StoreTDResultsInCache  "False"
     .RunDiscretizerOnly "False"
     .FullDeembedding "False"
     .SuperimposePLWExcitation "False"
     .UseSensitivityAnalysis "False"
End With

'@ set PBA version

'[VERSION]2024.1|33.0.1|20231016[/VERSION]
Discretizer.PBAVersion "2023101624"

