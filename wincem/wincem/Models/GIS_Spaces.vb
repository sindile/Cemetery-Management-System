Public Class GIS_Spaces
    Public Property displayFieldName() As String
    Public Property fieldAliases() As FieldAliases
    Public Property geometryType() As String
    Public Property spatialReference() As SpatialReference
    Public Property fields As List(Of Field)
    Public Property features() As List(Of Feature)
    Public Property exceededTransferLimit() As Boolean
End Class

Public Class FieldAliases
    Public Property OBJECTID() As String
    Public Property BURIALCD() As String
    Public Property Shape_Leng() As String
    Public Property Cemetery() As String
    Public Property Section_Di() As String
    Public Property Block() As String
    Public Property Lot_Row() As String
    Public Property SpaceNumbe() As String
    Public Property SpaceInfo() As String
    Public Property GIS() As String
    Public Property Space_ID() As String
    Public Property Space_Number() As String
    Public Property Lot_Row_ID() As String
    Public Property Lot_Row_Name() As String
    Public Property Block_Section_ID() As String
    Public Property Block_Section_Name() As String
    Public Property Division_Name() As String
    Public Property Division_ID() As String
    Public Property Cemetery_Name() As String
    Public Property Cemetery_ID() As String
    Public Property Space_Type() As String
    Public Property Available() As String
    Public Property SHAPE_STArea() As String
    Public Property SHAPE_STLength() As String
End Class

Public Class SpatialReference
    Public Property wkid() As Integer?
    Public Property latestWkid() As Integer?
End Class

Public Class Field

    Public Property name() As String
    Public Property type() As String
    Public Property _alias() As String
    Public Property length() As Integer?
End Class

Public Class Attributes
    Public Property OBJECTID() As Integer?
    Public Property BURIALCD() As String
    Public Property Shape_Leng() As Double?
    Public Property Cemetery() As String
    Public Property Section_Di() As String
    Public Property Block() As String
    Public Property Lot_Row() As String
    Public Property SpaceNumbe() As String
    Public Property SpaceInfo() As String
    Public Property GIS() As String
    Public Property Space_ID() As String
    Public Property Space_Number() As String
    Public Property Lot_Row_ID() As String
    Public Property Lot_Row_Name() As String
    Public Property Block_Section_ID() As String
    Public Property Block_Section_Name() As String
    Public Property Division_Name() As String
    Public Property Division_ID() As String
    Public Property Cemetery_Name() As String
    Public Property Cemetery_ID() As Integer?
    Public Property Space_Type() As String
    Public Property Available() As String
    Public Property SHAPE_STArea() As Double?
    Public Property SHAPE_STLength() As Double?
End Class

Public Class Geometry
    Public Property rings() As List(Of List(Of List(Of Double)))
End Class

Public Class Feature
    Public Property attributes() As Attributes
    Public Property geometry() As Geometry
End Class