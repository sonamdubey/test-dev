IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InsertPriceQuotes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InsertPriceQuotes]
GO

	/*
	Author:Vishal Srivastava AE1830
	Date:04/10/2013
	Description: Bind field of repeater made
*/
CREATE  PROCEDURE [dbo].[TC_InsertPriceQuotes]
@myTableType TC_CarVersionPQType READONLY
AS
BEGIN	
  
    
               UPDATE PQ
                       SET PQ.Value=M.Value,
                           PQ.TC_UserID=M.TC_UserId
                     FROM TC_CarVersionPQ AS  PQ
					 INNER JOIN @myTableType AS M ON PQ.TC_CarVersionPQId=M.TC_CarVersionPQId
    
       
                       INSERT INTO TC_CarVersionPQ (DealerId,
                                                    VersionId,
													TC_PQFieldMasterId,
													TC_UserID,
													Value)
                                       SELECT        M.DealerId,
                                                     M.VersionId,
                                                     M.TC_PQFieldMasterId,
                                                     M.TC_UserId,
                                                     M.Value
                                       FROM        @myTableType AS M
									   WHERE   TC_CarVersionPQId=-1 --- For insertion -1 will be pass from fornt end.

END
