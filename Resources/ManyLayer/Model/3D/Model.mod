'# MWS Version: Version 2024.1 - Oct 16 2023 - ACIS 33.0.1 -

'# length = mm
'# frequency = GHz
'# time = ns
'# frequency range: fmin = 1 fmax = 3
'# created = '[VERSION]2024.1|33.0.1|20231016[/VERSION]


'@ define material: Copper (pure)

'[VERSION]2024.1|33.0.1|20231016[/VERSION]
With Material
     .Reset
     .Name "Copper (pure)"
     .Folder ""
     .FrqType "all"
     .Type "Lossy metal"
     .MaterialUnit "Frequency", "GHz"
     .MaterialUnit "Geometry", "mm"
     .MaterialUnit "Time", "s"
     .MaterialUnit "Temperature", "Kelvin"
     .Mu "1.0"
     .Sigma "5.96e+007"
     .Rho "8930.0"
     .ThermalType "Normal"
     .ThermalConductivity "401.0"
     .SpecificHeat "390", "J/K/kg"
     .MetabolicRate "0"
     .BloodFlow "0"
     .VoxelConvection "0"
     .MechanicsType "Isotropic"
     .YoungsModulus "120"
     .PoissonsRatio "0.33"
     .ThermalExpansionRate "17"
     .ReferenceCoordSystem "Global"
     .CoordSystemType "Cartesian"
     .NLAnisotropy "False"
     .NLAStackingFactor "1"
     .NLADirectionX "1"
     .NLADirectionY "0"
     .NLADirectionZ "0"
     .FrqType "static"
     .Type "Normal"
     .SetMaterialUnit "Hz", "mm"
     .Epsilon "1"
     .Mu "1.0"
     .Kappa "5.96e+007"
     .TanD "0.0"
     .TanDFreq "0.0"
     .TanDGiven "False"
     .TanDModel "ConstTanD"
     .KappaM "0"
     .TanDM "0.0"
     .TanDMFreq "0.0"
     .TanDMGiven "False"
     .TanDMModel "ConstTanD"
     .DispModelEps "None"
     .DispModelMu "None"
     .DispersiveFittingSchemeEps "Nth Order"
     .DispersiveFittingSchemeMu "Nth Order"
     .UseGeneralDispersionEps "False"
     .UseGeneralDispersionMu "False"
     .Colour "1", "1", "0"
     .Wireframe "False"
     .Reflection "False"
     .Allowoutline "True"
     .Transparentoutline "False"
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
     .Material "Copper (pure)" 
     .Xrange "0", "50" 
     .Yrange "0", "1" 
     .Zrange "0", "50" 
     .Create
End With

'@ define material: Nickel

'[VERSION]2024.1|33.0.1|20231016[/VERSION]
With Material
     .Reset
     .Name "Nickel"
     .Folder ""
     .FrqType "all"
     .Type "Lossy metal"
     .SetMaterialUnit "GHz", "mm"
     .Mu "600"
     .Kappa "1.44e7"
     .Rho "8900"
     .ThermalType "Normal"
     .ThermalConductivity "91"
     .SpecificHeat "450", "J/K/kg"
     .MetabolicRate "0"
     .BloodFlow "0"
     .VoxelConvection "0"
     .MechanicsType "Isotropic"
     .YoungsModulus "207"
     .PoissonsRatio "0.31"
     .ThermalExpansionRate "13.1"
     .FrqType "static"
     .Type "Normal"
     .SetMaterialUnit "GHz", "mm"
     .Epsilon "1"
     .Mu "600"
     .Kappa "1.44e7"
     .TanD "0.0"
     .TanDFreq "0.0"
     .TanDGiven "False"
     .TanDModel "ConstTanD"
     .KappaM "0"
     .TanDM "0.0"
     .TanDMFreq "0.0"
     .TanDMGiven "False"
     .TanDMModel "ConstTanD"
     .DispModelEps "None"
     .DispModelMu "None"
     .DispersiveFittingSchemeEps "Nth Order"
     .DispersiveFittingSchemeMu "Nth Order"
     .UseGeneralDispersionEps "False"
     .UseGeneralDispersionMu "False"
     .Colour "0", "0.501961", "0.25098"
     .Wireframe "False"
     .Reflection "False"
     .Allowoutline "True"
     .Transparentoutline "False"
     .Transparency "0"
     .Create
End With

'@ define brick: component1:solid2

'[VERSION]2024.1|33.0.1|20231016[/VERSION]
With Brick
     .Reset 
     .Name "solid2" 
     .Component "component1" 
     .Material "Nickel" 
     .Xrange "0", "50" 
     .Yrange "1", "4" 
     .Zrange "0", "50" 
     .Create
End With

'@ define material: Polyimide (lossy)

'[VERSION]2024.1|33.0.1|20231016[/VERSION]
With Material
     .Reset
     .Name "Polyimide (lossy)"
     .Folder ""
     .FrqType "all"
     .Type "Normal"
     .SetMaterialUnit "MHz", "mm"
     .Epsilon "3.5"
     .Mu "1.0"
     .Kappa "0.0"
     .TanD "0.0027"
     .TanDFreq "1.0"
     .TanDGiven "True"
     .TanDModel "ConstTanD"
     .KappaM "0.0"
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
     .Rho "1400.0"
     .ThermalType "Normal"
     .ThermalConductivity "0.20"
     .SpecificHeat "1000", "J/K/kg"
     .SetActiveMaterial "all"
     .MechanicsType "Isotropic"
     .YoungsModulus "2.5"
     .PoissonsRatio "0.4"
     .ThermalExpansionRate "25"
     .Colour "0.94", "0.82", "0.76"
     .Wireframe "False"
     .Transparency "0"
     .Create
End With

'@ define brick: component1:solid3

'[VERSION]2024.1|33.0.1|20231016[/VERSION]
With Brick
     .Reset 
     .Name "solid3" 
     .Component "component1" 
     .Material "Polyimide (lossy)" 
     .Xrange "0", "50" 
     .Yrange "4", "6" 
     .Zrange "0", "50" 
     .Create
End With

'@ define frequency range

'[VERSION]2024.1|33.0.1|20231016[/VERSION]
Solver.FrequencyRange "1", "3"

'@ define plane wave properties

'[VERSION]2024.1|33.0.1|20231016[/VERSION]
With PlaneWave
     .Reset 
     .Normal "0", "-1", "0" 
     .EVector "100", "0", "0" 
     .Polarization "Linear" 
     .ReferenceFrequency "2" 
     .PhaseDifference "-90.0" 
     .CircularDirection "Left" 
     .AxialRatio "0.0" 
     .SetUserDecouplingPlane "False" 
     .Store
End With

'@ define probe: 0

'[VERSION]2024.1|33.0.1|20231016[/VERSION]
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
     .SetSubvolume "0", "50", "0", "6", "0", "50" 
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
     .SetSubvolume "0", "50", "0", "6", "0", "50" 
     .SetSubvolumeOffset "0.0", "0.0", "0.0", "0.0", "0.0", "0.0" 
     .SetSubvolumeInflateWithOffset "False" 
     .Create 
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

