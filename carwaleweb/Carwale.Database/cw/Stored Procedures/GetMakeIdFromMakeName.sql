IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetMakeIdFromMakeName]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetMakeIdFromMakeName]
GO

	
--Author: Ravi Koshal  
--Date:16 July 2013  
--Desc: Get ModelId for the corresponding model name.
CREATE PROCEDURE [cw].[GetMakeIdFromMakeName]   -- Exec cw.GetMakeIdFromMakeName 'marutisuzuki'

@MakeName varchar(30)  

AS  

BEGIN  


SELECT * FROM CarMakes WITH (NOLOCK)

where (select lower( [AC].[RemoveSpecialCharacters](Name))) = @MakeName 

END



