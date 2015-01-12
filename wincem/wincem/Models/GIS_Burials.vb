Namespace GIS_Burials
    Public Class GIS_Burials
        Public Property objectIdFieldName() As String
        Public Property globalIdFieldName() As String
        Public Property geometryType() As String
        Public Property spatialReference() As SpatialReference
        Public Property fields() As List(Of Field)
        Public Property features() As List(Of Feature)
    End Class
    Public Class SpatialReference
        Public Property wkid() As Integer
        Public Property latestWkid() As Integer
    End Class

    Public Class Field
        Public Property name() As String
        Public Property [alias]() As String
        Public Property type() As String
        Public Property length() As System.Nullable(Of Integer)
    End Class

    Public Class Geometry
        Public Property rings() As List(Of List(Of List(Of Double)))
    End Class
    Public Class Attributes
        Public Property OBJECTID() As Integer
        Public Property Burial_ID() As Integer
        Public Property Space_ID() As String
    End Class
    Public Class Feature
        Public Property geometry() As Geometry
        Public Property attributes() As Attributes
    End Class
End Namespace
