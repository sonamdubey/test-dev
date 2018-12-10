IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UsedCarFetchMakesAndRoots]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UsedCarFetchMakesAndRoots]
GO

	

-- ============================================= 
-- Author: Manish Chourasiya    
-- Create date: <12 AUG 2014> 
-- Description: To Fetch Car Makes & Roots
-- Modified by Shikhar on 12 AUG 2014
-- ============================================= 

CREATE PROCEDURE [dbo].[UsedCarFetchMakesAndRoots]
@MakeIdList VARCHAR(100) = '',
@RootIdList VARCHAR(100) = ''

AS

BEGIN

DECLARE @str VARCHAR(1000)

SELECT @str= coalesce(@str + ', ', '') + Roots.Value FROM -- Added this line to combine the result into a single string
	(
		SELECT 
			CK.Name+' '+CR.RootName + '|' + (CONVERT(VARCHAR,CK.ID) + '.' + CONVERT(VARCHAR,CR.RootId)) AS Value
		FROM 
			CarModelRoots AS CR WITH (NOLOCK)
				INNER JOIN fnSplitCSV(@RootIdList) AS F 
					ON F.ListMember=CR.RootId
				INNER JOIN CarMakes AS CK WITH (NOLOCK) 
					ON CR.MakeId=CK.ID
		UNION
		SELECT 
			CK.Name + '|' + CONVERT(VARCHAR,CK.ID) Value
		FROM 
			CarMakes AS CK WITH (NOLOCK)
				INNER JOIN fnSplitCSV(@MakeIdList) AS F 
					ON F.ListMember=CK.Id
	) AS Roots

SELECT @str

END
