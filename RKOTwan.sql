SELECT COUNT(*) AS NBRE,[C_id]
      
  FROM t_beneficiaires
    GROUP BY [C_id]
	HAVING COUNT(*)>1