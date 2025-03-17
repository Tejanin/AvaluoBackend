use Avaluo
INSERT INTO Estado (idtabla, descripcion) VALUES
('Rubrica', 'Activa y sin entregar'),
('Rubrica', 'Activa y entregada'),
('Rubrica', 'Entregada'),
('Rubrica', 'No entregada'),
('Usuario','Activo'),
('Usuario','Desactivado'),
('Asignatura', 'Activa'),
('Asignatura', 'Desactivada'),
('Competencia', 'Activa'),
('Competencia', 'Desactivada'),
('MapaCompetencia','Evaluando'),
('MapaCompetencia','No Evaluando'),
('Configuracion','Activa'),
('Configuracion','Desactivada'),
('Carrera', 'Activa'),
('Carrera', 'Desactivada'),
('Edificio', 'Activa'),
('Edificio', 'En Mantenimiento'),
('Edificio', 'Desactivada'),
('Aula', 'Activa'),
('Aula', 'En Mantenimiento'),
('Aula', 'Desactivada'),
('Inventario', 'Disponible'),
('Inventario', 'En Uso'),
('Inventario', 'En Mantenimiento'),
('Inventario', 'Baja');

INSERT INTO metodo_evaluacion (descripcionES, descripcionEN) VALUES
('Examen', 'Exam'),
('Tarea', 'Assignment'),
('Proyecto', 'Project'),
('Presentación', 'Presentation');

INSERT INTO roles (Descripcion, EsProfesor, EsSupervisor, EsCoordinadorArea, EsCoordinadorCarrera, EsAdmin, EsAux, VerInformes, VerListaDeRubricas, ConfigurarFechas, VerManejoCurriculum) VALUES
('Profesor', 1, 0, 0, 0, 0, 0, 1, 1, 0, 1),
('Administrador', 0, 0, 0, 0, 1, 0, 1, 1, 1, 1),
('Coordinador', 0, 0, 1, 1, 0, 0, 1, 1, 1, 1),
('Supervisor', 0, 1, 0, 0, 0, 0, 1, 1, 1, 1),
('Auxiliar', 0, 0, 0, 0, 0, 1, 0, 0, 0, 0),
('Prueba', 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);

INSERT INTO tipo_competencia (Nombre) VALUES
('Específica'),
('General'),
('Negocios')

INSERT INTO [dbo].[areas] ([Descripcion], [IdCoordinador], [FechaCreacion], [UltimaEdicion])  VALUES  
( 'Área de Ingeniería', NULL, GETDATE(), NULL),
( 'Área de Ciencias Básicas', NULL, GETDATE(), NULL), 
( 'Área de Negocios', NULL, GETDATE(), NULL),
( 'Área de Idiomas', NULL, GETDATE(), NULL),
( 'Área de Humanidades', NULL, GETDATE(), NULL)

INSERT INTO Asignaturas (Creditos, Codigo, Nombre, FechaCreacion, UltimaEdicion, IdEstado, ProgramaAsignatura, Syllabus, IdArea) 
VALUES 
(4, 'AHC109', 'REDACCION', GETDATE(), GETDATE(), 7, 'Fundamentos de la Escritura Académica', 'https://syllabus.universidad.edu/redaccion.pdf', 5),
(5, 'CBM101', 'ALGEBRA Y GEOMETRIA ANALITICA', GETDATE(), GETDATE(), 7, 'Conceptos básicos de álgebra y geometría', 'https://syllabus.universidad.edu/algebra.pdf', 2),
(2, 'CBA106', 'AMBIENTE Y CULTURA', GETDATE(), GETDATE(), 7, 'Estudio de la relación entre el ambiente y la sociedad', 'Por definir', 2),
(2, 'CCP103', 'CREATIVIDAD', GETDATE(), GETDATE(), 7, 'Desarrollo de habilidades creativas', 'https://syllabus.universidad.edu/creatividad.pdf', 5),
(2, 'IDS207', 'INTRODUCCION A LA INGENIERIA DE SOFTWARE', GETDATE(), GETDATE(), 7, 'Conceptos básicos de ingeniería de software', 'https://syllabus.universidad.edu/intro_software.pdf', 1),
(4, 'AHC110', 'ARGUMENTACION LINGUISTICA', GETDATE(), GETDATE(), 7, 'Estrategias para la argumentación efectiva', 'https://syllabus.universidad.edu/argumentacion.pdf', 5),
(5, 'CBM102', 'CALCULO DIFERENCIAL', GETDATE(), GETDATE(), 7, 'Introducción a derivadas y aplicaciones', 'https://syllabus.universidad.edu/calculo_dif.pdf', 2),
(4, 'IDS323', 'TECNICAS FUNDAMENTALES DE INGENIERIA DE SOFTWARE', GETDATE(), GETDATE(), 7, 'Principales metodologías de desarrollo', 'Por definir', 1),
(2, 'ING102', 'INTRODUCCION A LA PROGRAMACION', GETDATE(), GETDATE(), 7, 'Principios básicos de programación', 'https://syllabus.universidad.edu/intro_prog.pdf', 1),
(4, 'CBF210', 'FISICA MECANICA I', GETDATE(), GETDATE(), 7, 'Conceptos de cinemática y dinámica', 'https://syllabus.universidad.edu/fisica_mec.pdf', 2),
(5, 'CBM201', 'CALCULO INTEGRAL', GETDATE(), GETDATE(), 7, 'Integrales y aplicaciones', 'https://syllabus.universidad.edu/calculo_int.pdf', 2),
(4, 'IDS202', 'TECNOLOGIA DE OBJETOS', GETDATE(), GETDATE(), 7, 'Introducción a la programación orientada a objetos', 'https://syllabus.universidad.edu/tecn_objetos.pdf', 1),
(3, 'IDS340', 'DESARROLLO DE SOFTWARE I', GETDATE(), GETDATE(), 7, 'Fundamentos del desarrollo de software', 'https://syllabus.universidad.edu/desarrollo_sw1.pdf', 1),
(5, 'CBM202', 'CALCULO VECTORIAL', GETDATE(), GETDATE(), 7, 'Cálculo en varias variables', 'Por definir', 2),
(3, 'IDS341', 'DESARROLLO DE SOFTWARE II', GETDATE(), GETDATE(), 7, 'Aplicaciones avanzadas en desarrollo de software', 'https://syllabus.universidad.edu/desarrollo_sw2.pdf', 1),
(4, 'IDS208', 'TEAM BUILDING', GETDATE(), GETDATE(), 7, 'Habilidades para el trabajo en equipo', 'https://syllabus.universidad.edu/team_building.pdf', 1),
(5, 'CBM208', 'ALGEBRA LINEAL', GETDATE(), GETDATE(), 7, 'Espacios vectoriales y transformaciones lineales', 'Por definir', 2),
(3, 'IDS311', 'PROCESO DE SOFTWARE', GETDATE(), GETDATE(), 7, 'Ciclo de vida del software', 'https://syllabus.universidad.edu/proceso_software.pdf', 1),
(4, 'IDS324', 'INGENIERIA DE REQUERIMIENTOS DE SOFTWARE', GETDATE(), GETDATE(), 7, 'Gestión y documentación de requisitos', 'https://syllabus.universidad.edu/req_sw.pdf', 1),
(1, 'IDS343L','LABORATORIO ESTRUCTURAS DE DATOS Y ALGORITMOS I', GETDATE(), GETDATE(), 7, 'Algoritmos y estructuras de datos fundamentales', 'https://syllabus.universidad.edu/estructuras_datos1.pdf', 1),
(4, 'INS377', 'BASES DE DATOS I', GETDATE(), GETDATE(), 7, 'Algoritmos y estructuras de datos fundamentales', 'https://syllabus.universidad.edu/estructuras_datos1.pdf', 1),
(1, 'INS377L', 'LABORATORIO BASES DE DATOS I', GETDATE(), GETDATE(), 7, 'Algoritmos y estructuras de datos fundamentales', 'https://syllabus.universidad.edu/estructuras_datos1.pdf', 1),
(3, 'IDS345', 'DESARROLLO DE SOFTWARE III', GETDATE(), GETDATE(), 7, 'Algoritmos y estructuras de datos fundamentales', 'https://syllabus.universidad.edu/estructuras_datos1.pdf', 1),
(1, 'IDS345L', 'LABORATORIO DE DESARROLLO DE SOFTWARE III', GETDATE(), GETDATE(), 7, 'Algoritmos y estructuras de datos fundamentales', 'https://syllabus.universidad.edu/estructuras_datos1.pdf', 1),
(3, 'IDS346', 'MODELOS Y METODOS DE LA INGENIERIA DE SOFTWARE', GETDATE(), GETDATE(), 7, 'Algoritmos y estructuras de datos fundamentales', 'https://syllabus.universidad.edu/estructuras_datos1.pdf', 1),
(4, 'IDS329', 'INGENIERIA DE FACTORES HUMANOS', GETDATE(), GETDATE(), 7, 'Algoritmos y estructuras de datos fundamentales', 'https://syllabus.universidad.edu/estructuras_datos1.pdf', 1),
(4, 'INS380', 'BASES DE DATOS II', GETDATE(), GETDATE(), 7, 'Algoritmos y estructuras de datos fundamentales', 'https://syllabus.universidad.edu/estructuras_datos1.pdf', 1),
(1, 'INS380L', 'LABORATORIO DE BASES DE DATOS II', GETDATE(), GETDATE(), 7, 'Algoritmos y estructuras de datos fundamentales', 'https://syllabus.universidad.edu/estructuras_datos1.pdf', 1)


INSERT INTO Competencia (Nombre, Acron, Titulo, DescripcionES, DescripcionEN, id_tipo, id_estado, FechaCreacion, UltimaEdicion) VALUES
('Student Outcome 1', 'SO1', 'Resolución de problemas complejos de ingeniería', 
 'Capacidad para identificar, formular y resolver problemas complejos de ingeniería aplicando principios de ingeniería, ciencia y matemáticas.', 
 'Ability to identify, formulate, and solve complex engineering problems by applying principles of engineering, science, and mathematics.', 
 1, 9, GETDATE(), GETDATE()),

('Student Outcome 2', 'SO2', 'Diseño de soluciones de ingeniería', 
 'Capacidad para aplicar el diseño de ingeniería para producir soluciones que satisfagan necesidades específicas, considerando factores de salud pública, seguridad y bienestar, así como factores globales, culturales, sociales, ambientales y económicos.', 
 'Ability to apply engineering design to produce solutions that meet specified needs with consideration of public health, safety, and welfare, as well as global, cultural, social, environmental, and economic factors.', 
 1, 9, GETDATE(), GETDATE()),

('Student Outcome 3', 'SO3', 'Comunicación efectiva', 
 'Capacidad para comunicarse de manera efectiva con diferentes audiencias.', 
 'Ability to communicate effectively with a range of audiences.', 
 1, 9, GETDATE(), GETDATE()),

('Student Outcome 4', 'SO4', 'Responsabilidad ética y profesionalismo', 
 'Capacidad para reconocer responsabilidades éticas y profesionales en situaciones de ingeniería y hacer juicios informados, considerando el impacto de las soluciones en contextos globales, económicos, ambientales y sociales.', 
 'Ability to recognize ethical and professional responsibilities in engineering situations and make informed judgments, considering the impact of engineering solutions in global, economic, environmental, and societal contexts.', 
 1, 9, GETDATE(), GETDATE()),

('Student Outcome 5', 'SO5', 'Trabajo en equipo y liderazgo', 
 'Capacidad para trabajar eficazmente en un equipo cuyos miembros proporcionan liderazgo, crean un entorno colaborativo e inclusivo, establecen objetivos, planifican tareas y cumplen con los objetivos.', 
 'Ability to function effectively on a team whose members provide leadership, create a collaborative and inclusive environment, establish goals, plan tasks, and meet objectives.', 
 1, 9, GETDATE(), GETDATE()),

('Student Outcome 6', 'SO6', 'Experimentación y análisis de datos', 
 'Capacidad para desarrollar y llevar a cabo experimentos adecuados, analizar e interpretar datos y usar el juicio de ingeniería para sacar conclusiones.', 
 'Ability to develop and conduct appropriate experimentation, analyze and interpret data, and use engineering judgment to draw conclusions.', 
 1, 9, GETDATE(), GETDATE()),

('Student Outcome 7', 'SO7', 'Aprendizaje continuo y adaptación', 
 'Capacidad para adquirir y aplicar nuevos conocimientos según sea necesario, utilizando estrategias de aprendizaje apropiadas.', 
 'Ability to acquire and apply new knowledge as needed, using appropriate learning strategies.', 
 1, 9, GETDATE(), GETDATE());
 

 INSERT INTO pi (Nombre, SO_id, descripcionEN, descripcionES) VALUES
('PI 1', 1, 'Define the problem', 'Define el problema'),
('PI 2', 1, 'Determine the causes of the problem', 'Determina las causas del problema'),
('PI 3', 1, 'Propose solutions to the problem by applying principles of engineering, science, and/or mathematics', 'Propone soluciones al problema aplicando principios de ingeniería, ciencia y/o matemáticas'),
('PI 4', 1, 'Justifies the selected solution', 'Justifica la solución seleccionada'),
('PI 1', 2, 'Identifies needs and converts them into objectives, criteria and design restrictions', 'Identifica necesidades y las convierte en objetivos, criterios y restricciones de diseño'),
('PI 2', 2, 'Generates alternatives based on engineering sciences, social sciences, economics, among others', 'Genera alternativas basadas en ciencias de la ingeniería, ciencias sociales, economía, entre otras'),
('PI 3', 2, 'Selects the best alternative', 'Selecciona la mejor alternativa'),
('PI 4', 2, 'Creates specifications, prototypes or other means of design communication', 'Crea especificaciones, prototipos u otros medios de comunicación del diseño'),
('PI 1', 3, 'Writes reports based on standard formats adapted to the target audience', 'Redacta informes basados en formatos estándar adaptados al público objetivo'),
('PI 2', 3, 'Presents the content orally in his/her own words and adapts it to the audience present', 'Presenta el contenido oralmente con sus propias palabras y lo adapta a la audiencia presente'),
('PI 3', 3, 'Complements the information using graphs, diagrams, among others, according to the audience', 'Complementa la información utilizando gráficos, diagramas, entre otros, según la audiencia'),
('PI 4', 3, 'Provides the information with sustainable evidence', 'Proporciona la información con evidencia sustentable'),
('PI 1', 4, 'Develops solutions in accordance with current reality, taking into account ethical and professional responsibility', 'Desarrolla soluciones de acuerdo con la realidad actual, tomando en cuenta la responsabilidad ética y profesional'),
('PI 2', 4, 'Assesses the consequences of the impact of engineering decisions in global, regional and local contexts (economic, environmental and social)', 'Evalúa las consecuencias del impacto de las decisiones de ingeniería en contextos globales, regionales y locales (económico, ambiental y social)'),
('PI 3', 4, 'Recognizes the copyright in the particular solutions developed', 'Reconoce los derechos de autor en las soluciones particulares desarrolladas'),
('PI 1', 5, 'Plans strategies to meet objectives', 'Planifica estrategias para cumplir objetivos'),
('PI 2', 5, 'Interacts and communicates with team members, open to the opinions of others', 'Interactúa y se comunica con los miembros del equipo, abierto a las opiniones de otros'),
('PI 3', 5, 'Identifies and fulfills his role as a member of the work team to achieve the objectives', 'Identifica y cumple su rol como miembro del equipo de trabajo para lograr los objetivos'),
('PI 1', 6, 'Plan or design the experiment', 'Planifica o diseña el experimento'),
('PI 2', 6, 'Conduct the experiment', 'Realiza el experimento'),
('PI 3', 6, 'Analyze and interpret the results', 'Analiza e interpreta los resultados'),
('PI 4', 6, 'Discuss the results obtained from the experiment', 'Discute los resultados obtenidos del experimento'),
('PI 1', 7, 'Need to acquire new knowledge', 'Necesidad de adquirir nuevos conocimientos'),
('PI 2', 7, 'Relationship between knowledge and learning strategy', 'Relación entre conocimiento y estrategia de aprendizaje'),
('PI 3', 7, 'Obtaining new concepts', 'Obtención de nuevos conceptos');

-- Datos de prueba para mapa competencias
INSERT INTO [dbo].[mapa_competencias] ([Id_Asignatura], [Id_Competencia], [Id_Estado]) 
VALUES
(5, 1, 11),
(8, 2, 11),
(12, 3, 11),
(15, 4, 11),
(18, 5, 11),
(21, 6, 11),
(23, 7, 11),
(25, 1, 11),
(19, 2, 11),
(26, 3, 11),
-- Las asignaturas que no tienen rúbricas, las colocamos en "No Evaluando" (IdEstado 12)
(1, 7, 12),  -- REDACCION
(2, 7, 12),  -- ALGEBRA Y GEOMETRIA ANALITICA
(3, 7, 12),  -- AMBIENTE Y CULTURA
(4, 7, 12),  -- CREATIVIDAD
(6, 7, 12),  -- ARGUMENTACION LINGUISTICA
(7, 7, 12),  -- CALCULO DIFERENCIAL
(9, 7, 12),  -- INTRODUCCION A LA PROGRAMACION
(10, 7, 12), -- FISICA MECANICA I
(11, 7, 12), -- CALCULO INTEGRAL
(13, 7, 12), -- DESARROLLO DE SOFTWARE I
(14, 7, 12), -- CALCULO VECTORIAL
(16, 7, 12), -- TEAM BUILDING
(17, 7, 12), -- ALGEBRA LINEAL
(20, 7, 12), -- LABORATORIO ESTRUCTURAS DE DATOS Y ALGORITMOS I
(22, 7, 12), -- LABORATORIO BASES DE DATOS I
(24, 7, 12), -- LABORATORIO DE DESARROLLO DE SOFTWARE III
(27, 7, 12), -- BASES DE DATOS II
(28, 7, 12); -- LABORATORIO DE BASES DE DATOS II

INSERT INTO so_evaluacion (Id_SO, Id_MetodoEvaluacion) VALUES
(1, 2),
(1, 3),
(1, 4),
(2, 1),
(2, 2),
(2, 3),
(2, 4),
(3, 1),
(3, 2),
(3, 4),
(4, 1),
(4, 2),
(5, 2),
(5, 3),
(6, 1),
(6, 2),
(6, 3),
(7, 1),
(7, 3),
(7, 4);

INSERT INTO [dbo].[carreras] ([IdArea], [Año], [NombreCarrera], [IdCoordinadorCarrera], [PEOs], [FechaCreacion], [UltimaEdicion] ,[IdEstado]) VALUES
           (1,2020,'Ingeniería de Software',null,
		   'Ser líderes en organizaciones locales e internacionales, ejerciendo la ingeniería industrial en un marco de integridad, excelencia y responsabilidad social.
			Generar soluciones integradas y sustentables para la industria considerando la actualidad y el contexto global.
			Lograr el desarrollo profesional y el crecimiento del conocimiento a través de certificaciones internacionales, estudios de posgrado, experiencia laboral y autoaprendizaje.',
			GETDATE(),null,15),
			(1,2020,'Ingeniería en Sistemas',null,
		   'Ser líderes en organizaciones locales e internacionales, ejerciendo la ingeniería industrial en un marco de integridad, excelencia y responsabilidad social.
			Generar soluciones integradas y sustentables para la industria considerando la actualidad y el contexto global.
			Lograr el desarrollo profesional y el crecimiento del conocimiento a través de certificaciones internacionales, estudios de posgrado, experiencia laboral y autoaprendizaje.',
			GETDATE(),null,15);

INSERT INTO [dbo].[asignatura_carrera] ([Id_Asignatura],[Id_Carrera]) VALUES
           (5,1),
           (8,1),
           (12,1), 
           (13,1), 
           (15,1),
           (12,2),
           (13,2),
           (15,2),
           (16,1),
           (18,1),
           (19,1), 
           (19,2),
           (20,1),
           (21,1), 
           (22,1), 
           (23,1), 
           (24,1), 
           (21,2), 
           (22,2), 
           (23,2), 
           (24,2),
		   (25, 1),
		   (26, 1),
		   (27, 2),
		   (28, 2); 

INSERT INTO [dbo].[desempeno] 
([Id], [Id_SO], [IdPI], [Id_Asignatura], [Satisfactorio], [Trimestre], [Año], [Porcentaje])
VALUES
-- Rúbrica 1 (SO 1) - Asignatura 5 - CantEstudiantes: 30
-- CantSatisfactorio + CantExperto:
-- PI 1: 7 + 5 = 12 --> (12 / 30) * 100 = 40.00
(1, 1, 1, 5, 0, 1, 2025, 40.00),  
-- PI 2: 6 + 4 = 10 --> (10 / 30) * 100 = 33.33
(2, 1, 2, 5, 0, 2, 2025, 33.33),  
-- PI 3: 9 + 3 = 12 --> (12 / 30) * 100 = 40.00
(3, 1, 3, 5, 0, 3, 2025, 40.00),
-- PI 4: 8 + 6 = 14 --> (14 / 30) * 100 = 46.67
(4, 1, 4, 5, 0, 4, 2025, 46.67),
-- Rúbrica 2 (SO 2) - Asignatura 8 - CantEstudiantes: 25
-- PI 5: 5 + 7 = 12 --> (12 / 25) * 100 = 48.00
(5, 2, 5, 8, 0, 1, 2025, 48.00),
-- PI 6: 7 + 5 = 12 --> (12 / 25) * 100 = 48.00
(6, 2, 6, 8, 0, 2, 2025, 48.00),
-- PI 7: 6 + 4 = 10 --> (10 / 25) * 100 = 40.00
(7, 2, 7, 8, 0, 3, 2025, 40.00),
-- PI 8: 8 + 6 = 14 --> (14 / 25) * 100 = 56.00
(8, 2, 8, 8, 1, 4, 2025, 56.00),
-- Rúbrica 3 (SO 3) - Asignatura 12 - CantEstudiantes: 28
-- PI 9: 7 + 5 = 12 --> (12 / 28) * 100 ≈ 42.86
(9, 3, 9, 12, 0, 1, 2025, 42.86),
-- PI 10: 8 + 6 = 14 --> (14 / 28) * 100 = 50.00
(10, 3, 10, 12, 1, 2, 2025, 50.00),
-- PI 11: 6 + 7 = 13 --> (13 / 28) * 100 ≈ 46.43
(11, 3, 11, 12, 0, 3, 2025, 46.43),
-- PI 12: 5 + 4 = 9 --> (9 / 28) * 100 ≈ 32.14
(12, 3, 12, 12, 0, 4, 2025, 32.14),
-- Rúbrica 4 (SO 4) - Asignatura 15 - CantEstudiantes: 32
-- PI 13: 6 + 7 = 13 --> (13 / 32) * 100 ≈ 40.63
(13, 4, 13, 15, 0, 1, 2025, 40.63),
-- PI 14: 7 + 3 = 10 --> (10 / 32) * 100 ≈ 31.25
(14, 4, 14, 15, 0, 2, 2025, 31.25),
-- PI 15: 8 + 6 = 14 --> (14 / 32) * 100 ≈ 43.75
(15, 4, 15, 15, 0, 3, 2025, 43.75),
-- Rúbrica 5 (SO 5) - Asignatura 18 - CantEstudiantes: 20
-- PI 16: 7 + 5 = 12 --> (12 / 20) * 100 = 60.00
(16, 5, 16, 18, 1, 1, 2025, 60.00),
-- PI 17: 8 + 4 = 12 --> (12 / 20) * 100 = 60.00
(17, 5, 17, 18, 1, 2, 2025, 60.00),
-- PI 18: 7 + 6 = 13 --> (13 / 20) * 100 = 65.00
(18, 5, 18, 18, 1, 3, 2025, 65.00),
-- Rúbrica 6 (SO 6) - Asignatura 20 - CantEstudiantes: 35
-- PI 19: 6 + 5 = 11 --> (11 / 35) * 100 ≈ 31.43
(19, 6, 19, 20, 0, 1, 2025, 31.43),
-- PI 20: 7 + 6 = 13 --> (13 / 35) * 100 ≈ 37.14
(20, 6, 20, 20, 0, 2, 2025, 37.14),
-- PI 21: 8 + 7 = 15 --> (15 / 35) * 100 ≈ 42.86
(21, 6, 21, 20, 0, 3, 2025, 42.86),
-- PI 22: 7 + 3 = 10 --> (10 / 35) * 100 ≈ 28.57
(22, 6, 22, 20, 0, 4, 2025, 28.57),
-- Rúbrica 7 (SO 7) - Asignatura 21 - CantEstudiantes: 27
-- PI 23: 7 + 5 = 12 --> (12 / 27) * 100 ≈ 44.44
(23, 7, 23, 21, 0, 1, 2024, 44.44),
-- PI 24: 6 + 6 = 12 --> (12 / 27) * 100 ≈ 44.44
(24, 7, 24, 21, 12, 2, 2024, 44.44),
-- PI 25: 8 + 4 = 12 --> (12 / 27) * 100 ≈ 44.44
(25, 7, 25, 21, 0, 3, 2024, 44.44);

-- Datos de prueba para rubricas
INSERT INTO [dbo].[rubricas]
           ([IdSO], [IdProfesor], [IdAsignatura], [IdEstado], [IdMetodoEvaluacion], [FechaCompletado], 
            [UltimaEdicion], [CantEstudiantes], [Año], [Periodo], [Seccion], [Comentario], [Problematica], 
            [Solucion], [Evidencia], [EvaluacionesFormativas], [Estrategias]) 
VALUES
(1, 3, 5, 1, 2, GETDATE(), GETDATE(), 30, 2025, '1', 'A', 'Buena participación', 
 'Dificultades con conceptos avanzados', 'Revisión grupal', 'Evidencia en informe final', 
 'Pruebas escritas', 'Tutorías individuales'),

(2, 4, 8, 2, 3, GETDATE(), GETDATE(), 25, 2025, '1', 'B', 'Evaluación regular', 
 'Problemas con la metodología de trabajo', 'Implementación de ejercicios guiados', 
 'Archivos PDF con análisis', 'Exámenes parciales', 'Uso de software especializado'),

(3, 5, 12, 3, 1, GETDATE(), GETDATE(), 28, 2025, '1', 'C', 'Resultados mixtos', 
 'Poca participación en debates', 'Integración de foros de discusión', 
 'Videos de presentaciones', 'Autoevaluaciones', 'Aprendizaje basado en proyectos'),

(4, 6, 15, 1, 4, GETDATE(), GETDATE(), 32, 2025, '1', 'D', 'Dificultad en aplicación práctica', 
 'Falta de experiencia con herramientas de desarrollo', 'Laboratorios adicionales', 
 'Reportes de pruebas', 'Pruebas en línea', 'Trabajo en equipo'),

(5, 7, 18, 4, 2, GETDATE(), GETDATE(), 20, 2025, '1', 'E', 'Avance satisfactorio', 
 'Requieren más material de apoyo', 'Entrega de material complementario', 
 'Resúmenes escritos', 'Evaluaciones orales', 'Casos de estudio'),

(6, 3, 20, 1, 3, GETDATE(), GETDATE(), 35, 2025, '1', 'F', 'Excelente desempeño', 
 'Algunos estudiantes no completan tareas', 'Refuerzo con sesiones de repaso', 
 'Videos explicativos', 'Cuestionarios en línea', 'Clases interactivas'),

(7, 4, 21, 3, 1, GETDATE(), GETDATE(), 27, 2024, '1', 'G', 'Desempeño variado', 
 'Falta de estructura en respuestas escritas', 'Prácticas de redacción técnica', 
 'Ejercicios de escritura', 'Corrección de ensayos', 'Revisión por pares'),

(1, 5, 23, 2, 4, GETDATE(), GETDATE(), 33, 2024, '1', 'H', 'Alto interés en la asignatura', 
 'Necesidad de mayor práctica en código', 'Programación en vivo', 
 'Capturas de código', 'Proyectos individuales', 'Desarrollo de software en equipo'),

(2, 6, 25, 1, 2, GETDATE(), GETDATE(), 29, 2024, '1', 'I', 'Falta de enfoque en los detalles', 
 'Dificultad en depuración de código', 'Ejercicios específicos de depuración', 
 'Capturas de pantalla', 'Simulaciones', 'Tareas prácticas'),

(3, 7, 26, 3, 3, GETDATE(), GETDATE(), 22, 2024, '1', 'J', 'Falta de dominio de ciertos temas', 
 'Falta de práctica con algoritmos recursivos', 'Ejercicios guiados', 
 'Códigos de muestra', 'Exámenes abiertos', 'Uso de herramientas online');

INSERT INTO [dbo].[resumen] ([Id_PI], [Id_Rubrica], [CantDesarrollo], [CantExperto], [CantPrincipiante], [CantSatisfactorio])
VALUES
 -- Rúbrica 1 (SO 1, PI 1-4)
(1, 1, 10, 5, 3, 7),
(2, 1, 8, 4, 2, 6),
(3, 1, 12, 3, 4, 9),
(4, 1, 11, 6, 3, 8),
--Rúbrica 2 (SO 2, PI 1-4)
(5, 2, 9, 7, 4, 5),
(6, 2, 13, 5, 3, 7),
(7, 2, 12, 4, 2, 6),
(8, 2, 10, 6, 5, 8),
-- Rúbrica 3 (SO 3, PI 1-4)
(9, 3, 11, 5, 4, 7),
(10, 3, 12, 6, 3, 8),
(11, 3, 8, 7, 5, 6),
(12, 3, 9, 4, 6, 5),
-- Rúbrica 4 (SO 4, PI 1-3)
(13, 4, 10, 7, 3, 6),
(14, 4, 15, 3, 4, 7),
(15, 4, 11, 6, 5, 8),
-- Rúbrica 5 (SO 5, PI 1-3)
(16, 5, 12, 5, 4, 7),
(17, 5, 14, 4, 3, 8),
(18, 5, 10, 6, 5, 7),
-- Rúbrica 6 (SO 6, PI 1-4)
(19, 6, 13, 5, 3, 6),
(20, 6, 12, 6, 4, 7),
(21, 6, 11, 7, 5, 8),
(22, 6, 14, 3, 2, 7),
-- Rúbrica 7 (SO 7, PI 1-3)
(23, 7, 12, 5, 3, 7),
(24, 7, 11, 6, 4, 6),
(25, 7, 13, 4, 2, 8),
-- Rúbrica 8 (SO 1, PI 1-4) -> Vuelve a SO 1
(1, 8, 10, 7, 3, 7),
(2, 8, 9, 6, 4, 6),
(3, 8, 11, 5, 3, 8),
(4, 8, 12, 4, 2, 9),
-- Rúbrica 9 (SO 2, PI 1-4)
(5, 9, 12, 6, 4, 7),
(6, 9, 10, 5, 3, 6),
(7, 9, 13, 4, 2, 8),
(8, 9, 11, 7, 5, 7),
-- Rúbrica 10 (SO 3, PI 1-4)S
(9, 10, 13, 5, 3, 7),
(10, 10, 14, 6, 4, 8),
(11, 10, 12, 7, 5, 6),
(12, 10, 11, 4, 3, 7);


/* Estas tablas no se estan usando

-- Datos de prueba para action_plan
INSERT INTO [dbo].[action_plan] 
([Id], [Id_Desempeno], [FechaCreacion], [UltimaEdicion], [Id_Rubrica], [Descripcion], [Id_Estado]) 
VALUES
(1, 1, GETDATE(), NULL, 1, 'Mejorar la calidad de la enseñanza en Ingeniería', 1),
(2, 2, GETDATE(), NULL, 2, 'Implementar estrategias para el desarrollo de software', 2);

-- Datos de prueba para tarea
INSERT INTO [dbo].[tarea] 
([Id], [Id_ActionPlan], [Id_Auxiliar], [Id_EstadoTarea], [FechaCreacion], [UltimaEdicion], [Descripcion]) 
VALUES
(1, 1, 1, 1, GETDATE(), NULL, 'Revisión de informes de desempeño'),
(2, 2, 2, 2, GETDATE(), NULL, 'Implementación de mejoras en evaluación');

*/

-- Datos de prueba para edificios
INSERT INTO [dbo].[edificios] 
([Id], [Id_Area], [Nombre], [FechaCreacion], [UltimaEdicion], [Acron], [Id_Estado], [Ubicacion]) 
VALUES
(1, 5, 'Edificio Ercilia Pepín', GETDATE(), GETDATE(), 'EP', 17, 'Campus INTEC, Santo Domingo'),
(2, 1, 'Laboratorio de Máquinas Eléctricas', GETDATE(), GETDATE(), 'LME', 17, 'Campus INTEC, Santo Domingo'),
(3, 3, 'Edificio De Ramón Picazo', GETDATE(), GETDATE(), 'DP', 17, 'Campus INTEC, Santo Domingo'),
(4, 1, 'Edificio Osvaldo García de la Concha', GETDATE(), GETDATE(), 'GC', 17, 'Campus INTEC, Santo Domingo'),
(5, 2, 'Edificio de Postgrado Eduardo Latorre', GETDATE(), GETDATE(), 'EL', 17, 'Campus INTEC, Santo Domingo'),
(6, 2, 'Edificio Fernando Defilló', GETDATE(), GETDATE(), 'FD', 17, 'Campus INTEC, Santo Domingo'),
(7, 4, 'Edificio Ana Mercedes Henríquez', GETDATE(), GETDATE(), 'AH', 17, 'Campus INTEC, Santo Domingo'),
(8, 5, 'Edificio Evangelina Rodríguez', GETDATE(), GETDATE(), 'ER', 17, 'Campus INTEC, Santo Domingo'),
(9, 2, 'Edificio Pedro Francisco Bonó', GETDATE(), GETDATE(), 'PB', 17, 'Campus INTEC, Santo Domingo'),
(10, 5, 'Edificio Arturo Jiménez Sabater', GETDATE(), GETDATE(), 'AJ', 17, 'Campus INTEC, Santo Domingo'),
(11, 3, 'Edificio Los Fundadores', GETDATE(), GETDATE(), 'LF', 17, 'Campus INTEC, Santo Domingo'),
(12, 3, 'Los Fundadores Anexo', GETDATE(), GETDATE(), 'LFA', 17, 'Campus INTEC, Santo Domingo');

-- Datos de prueba para aula
INSERT INTO [dbo].[aula] 
([Id], [Descripcion], [FechaCreacion], [UltimaEdicion], [Id_Edificio], [Id_Estado]) 
VALUES
-- Ercilia Pepín (EP)
(1, 'Aula EP-101', GETDATE(), GETDATE(), 1, 20),
(2, 'Aula EP-102', GETDATE(), GETDATE(), 1, 20),
(3, 'Laboratorio de Automatización Industrial', GETDATE(), GETDATE(), 1, 20),
-- Laboratorio Máquinas Eléctricas (LME)
(4, 'Laboratorio Máquinas Eléctricas 1', GETDATE(), GETDATE(), 2, 20),
-- De Ramón Picazo (DP)
(5, 'Aula DP-201', GETDATE(), GETDATE(), 3, 20),
(6, 'Taller MakerSpace', GETDATE(), GETDATE(), 3, 20),
-- Osvaldo García de la Concha (GC)
(7, 'Aula GC-301', GETDATE(), GETDATE(), 4, 20),
(8, 'Auditorio Osvaldo García de la Concha', GETDATE(), GETDATE(), 4, 20),
-- Postgrado Eduardo Latorre (EL)
(9, 'Aula EL-401', GETDATE(), GETDATE(), 5, 20),
(10, 'Sala de Conferencias EL', GETDATE(), GETDATE(), 5, 20),
-- Fernando Defilló (FD)
(11, 'Laboratorio de Biología', GETDATE(), GETDATE(), 6, 20),
(12, 'Laboratorio de Microbiología', GETDATE(), GETDATE(), 6, 20),
-- Ana Mercedes Henríquez (AH)
(13, 'Salón Múltiple AH', GETDATE(), GETDATE(), 7, 20),
(14, 'Salón de OSES', GETDATE(), GETDATE(), 7, 20),
-- Evangelina Rodríguez (ER)
(15, 'Aula ER-501', GETDATE(), GETDATE(), 8, 20),
(16, 'Unidad de Atención Primaria y LAB INTEC', GETDATE(), GETDATE(), 8, 20),
-- Pedro Francisco Bonó (PB)
(17, 'Aula PB-601', GETDATE(), GETDATE(), 9, 20),
(18, 'Laboratorio de Clínica Odontológica', GETDATE(), GETDATE(), 9, 20),
-- Arturo Jiménez Sabater (AJ)
(19, 'Aula AJ-701', GETDATE(), GETDATE(), 10, 20),
(20, 'Laboratorio de Matemática Aplicada', GETDATE(), GETDATE(), 10, 20),
-- Los Fundadores (LF)
(21, 'Aula LF-801', GETDATE(), GETDATE(), 11, 20),
(22, 'Sala de Videoconferencias', GETDATE(), GETDATE(), 11, 20),
-- Los Fundadores Anexo (LFA)
(23, 'Aula LFA-901', GETDATE(), GETDATE(), 12, 20),
(24, 'Laboratorio de Genética', GETDATE(), GETDATE(), 12, 20);

-- Datos de prueba para inventario
INSERT INTO [dbo].[inventario] 
([Id], [Descripcion], [FechaCreacion], [UltimaEdicion], [Id_Estado]) 
VALUES
(1, 'Proyector Epson X-40', GETDATE(), GETDATE(), 23),          -- Disponible
(2, 'Computadora Dell OptiPlex 7090', GETDATE(), GETDATE(), 24), -- En Uso
(3, 'Pizarra Acrílica 2x1m', GETDATE(), GETDATE(), 23),          -- Disponible
(4, 'Escritorio para profesor', GETDATE(), GETDATE(), 23),       -- Disponible
(5, 'Sillas de estudiante con paleta', GETDATE(), GETDATE(), 23),-- Disponible
(6, 'Laboratorio de Electrónica Kit Básico', GETDATE(), GETDATE(), 24), -- En Uso
(7, 'Microscopio Binocular', GETDATE(), GETDATE(), 25),          -- En Mantenimiento
(8, 'Equipo de Redes Cisco 2900', GETDATE(), GETDATE(), 23),     -- Disponible
(9, 'Impresora HP LaserJet Pro', GETDATE(), GETDATE(), 25),      -- En Mantenimiento
(10, 'Equipo de Realidad Virtual Oculus Rift', GETDATE(), GETDATE(), 24), -- En Uso
(11, 'Set de Instrumentos Odontológicos', GETDATE(), GETDATE(), 24), -- En Uso
(12, 'Sistema de Audio Logitech Z623', GETDATE(), GETDATE(), 23); -- Disponible

-- Datos de prueba para objeto_aula
INSERT INTO [dbo].[objeto_aula] 
([Id_Objeto], [Id_Aula], [Cantidad]) 
VALUES
-- Aulas en EP
(1, 1, 1), -- Proyector Epson X-40 en EP-101
(5, 1, 30), -- Sillas de estudiante
(4, 1, 1), -- Escritorio para profesor
(2, 2, 10), -- PC Dell en EP-102
(3, 2, 1), -- Pizarra Acrílica
(5, 2, 25),
(6, 3, 15), -- Lab Electrónica Kit en Lab Automatización EP
-- LME
(6, 4, 10), -- Lab Electrónica Kit
(3, 4, 1),
-- DP
(2, 5, 20), -- PC Dell en DP-201
(1, 5, 1),
(5, 5, 25),
(10, 6, 5), -- VR Oculus Rift en MakerSpace
(2, 6, 5),
-- GC
(1, 7, 1), -- Proyector
(5, 7, 20),
(4, 7, 1),
(8, 8, 3), -- Cisco 2900 en Auditorio GC
(9, 8, 1),
-- EL
(1, 9, 1), -- Proyector
(5, 9, 15),
(3, 10, 1), -- Pizarra en Sala de Conferencias EL
(5, 10, 10),
-- FD
(7, 11, 10), -- Microscopios en Lab Biología
(3, 11, 1),
(7, 12, 8), -- Microscopios en Lab Microbiología
(9, 12, 1),
-- AH
(1, 13, 1), -- Proyector en Salón Múltiple AH
(5, 13, 40),
(4, 14, 1), -- Escritorio en Salón OSES
(5, 14, 30),
-- ER
(11, 15, 10), -- Set de Instrumentos Odontológicos
(9, 15, 1),
(11, 16, 5), -- LAB INTEC
(2, 16, 10),
-- PB
(11, 17, 10), -- Clínica Odontológica
(3, 17, 1),
(1, 18, 1),
(12, 18, 2), -- Audio Logitech
-- AJ
(2, 19, 25), -- PCs en AJ-701
(1, 19, 1),
(2, 20, 20), -- Lab Matemática Aplicada
(3, 20, 1),
-- LF
(1, 21, 1),
(5, 21, 20),
(9, 22, 1), -- Videoconferencia
(3, 22, 1),
-- LFA
(11, 23, 8), -- Genética
(2, 23, 10),
(11, 24, 10),
(7, 24, 5); -- Microscopios en Genética

-- Datos de prueba para contacto
INSERT INTO [dbo].[contacto] 
([Id], [NumeroContacto], [Id_Usuario]) 
VALUES
-- profesor1
(1, '8095550101', 1),
(2, '8095550102', 1),
-- profesor2
(3, '8295550201', 2),
-- profesor3
(4, '8495550202', 3),
(5, '8095550301', 3),
-- profesor4
(6, '8095550401', 4),
-- admin1
(7, '8295550501', 5),
-- coordinador1
(8, '8495550601', 6),
(9, '8095550602', 6),
-- supervisor1
(10, '8095550701', 7),
-- auxiliar1
(11, '8095550801', 8),
(12, '8295550802', 8),
-- prueba1
(13, '8095550901', 9),
-- admin2
(14, '8095551001', 10);


-- Datos de prueba para evidencia
INSERT INTO [dbo].[evidencia] 
([Id], [Id_Rubrica], [Nombre], [Ruta], [FechaCreacion]) 
VALUES
-- Rubrica 1 (Informe Final de Proyecto)
(1, 1, 'Informe Final - Grupo 1', 'https://evidencias.intec.edu.do/rubrica1/grupo1_informe_final.pdf', GETDATE()),
(2, 1, 'Informe Final - Grupo 2', 'https://evidencias.intec.edu.do/rubrica1/grupo2_informe_final.pdf', GETDATE()),
(3, 1, 'Informe Final - Grupo 3', 'https://evidencias.intec.edu.do/rubrica1/grupo3_informe_final.pdf', GETDATE()),
-- Rubrica 2 (Prototipo Funcional)
(4, 2, 'Prototipo Funcional - Grupo A', 'https://evidencias.intec.edu.do/rubrica2/grupoA_prototipo.zip', GETDATE()),
(5, 2, 'Prototipo Funcional - Grupo B', 'https://evidencias.intec.edu.do/rubrica2/grupoB_prototipo.zip', GETDATE()),
(6, 2, 'Prototipo Funcional - Grupo C', 'https://evidencias.intec.edu.do/rubrica2/grupoC_prototipo.zip', GETDATE()),
-- Rubrica 3 (Video Presentación)
(7, 3, 'Video Presentación - Estudiante Ana', 'https://evidencias.intec.edu.do/rubrica3/ana_presentacion.mp4', GETDATE()),
(8, 3, 'Video Presentación - Estudiante Juan', 'https://evidencias.intec.edu.do/rubrica3/juan_presentacion.mp4', GETDATE()),
(9, 3, 'Video Presentación - Estudiante Luis', 'https://evidencias.intec.edu.do/rubrica3/luis_presentacion.mp4', GETDATE()),
-- Rubrica 4 (Informe de Prácticas)
(10, 4, 'Informe de Prácticas - Grupo Delta', 'https://evidencias.intec.edu.do/rubrica4/grupo_delta_practicas.pdf', GETDATE()),
(11, 4, 'Informe de Prácticas - Grupo Epsilon', 'https://evidencias.intec.edu.do/rubrica4/grupo_epsilon_practicas.pdf', GETDATE()),
(12, 4, 'Informe de Prácticas - Grupo Zeta', 'https://evidencias.intec.edu.do/rubrica4/grupo_zeta_practicas.pdf', GETDATE()),
-- Rubrica 5 (Caso de Estudio)
(13, 5, 'Caso de Estudio - Grupo 1', 'https://evidencias.intec.edu.do/rubrica5/grupo1_caso_estudio.pdf', GETDATE()),
(14, 5, 'Caso de Estudio - Grupo 2', 'https://evidencias.intec.edu.do/rubrica5/grupo2_caso_estudio.pdf', GETDATE()),
(15, 5, 'Caso de Estudio - Grupo 3', 'https://evidencias.intec.edu.do/rubrica5/grupo3_caso_estudio.pdf', GETDATE()),
-- Rubrica 6 (Video Explicativo)
(16, 6, 'Video Explicativo - Estudiante Pedro', 'https://evidencias.intec.edu.do/rubrica6/pedro_video.mp4', GETDATE()),
(17, 6, 'Video Explicativo - Estudiante María', 'https://evidencias.intec.edu.do/rubrica6/maria_video.mp4', GETDATE()),
(18, 6, 'Video Explicativo - Estudiante José', 'https://evidencias.intec.edu.do/rubrica6/jose_video.mp4', GETDATE()),
-- Rubrica 7 (Ensayo Técnico)
(19, 7, 'Ensayo Técnico - Grupo Omega', 'https://evidencias.intec.edu.do/rubrica7/grupo_omega_ensayo.pdf', GETDATE()),
(20, 7, 'Ensayo Técnico - Grupo Sigma', 'https://evidencias.intec.edu.do/rubrica7/grupo_sigma_ensayo.pdf', GETDATE()),
(21, 7, 'Ensayo Técnico - Grupo Lambda', 'https://evidencias.intec.edu.do/rubrica7/grupo_lambda_ensayo.pdf', GETDATE()),
-- Rubrica 8 (Proyecto Programación)
(22, 8, 'Proyecto Programación - Grupo Red', 'https://evidencias.intec.edu.do/rubrica8/grupo_red_proyecto.zip', GETDATE()),
(23, 8, 'Proyecto Programación - Grupo Blue', 'https://evidencias.intec.edu.do/rubrica8/grupo_blue_proyecto.zip', GETDATE()),
(24, 8, 'Proyecto Programación - Grupo Green', 'https://evidencias.intec.edu.do/rubrica8/grupo_green_proyecto.zip', GETDATE()),
-- Rubrica 9 (Ejercicios de Depuración)
(25, 9, 'Ejercicio Depuración - Estudiante Julia', 'https://evidencias.intec.edu.do/rubrica9/julia_depuracion.pdf', GETDATE()),
(26, 9, 'Ejercicio Depuración - Estudiante Ernesto', 'https://evidencias.intec.edu.do/rubrica9/ernesto_depuracion.pdf', GETDATE()),
(27, 9, 'Ejercicio Depuración - Estudiante Camila', 'https://evidencias.intec.edu.do/rubrica9/camila_depuracion.pdf', GETDATE()),
-- Rubrica 10 (Examen Abierto)
(28, 10, 'Examen Abierto - Grupo 1', 'https://evidencias.intec.edu.do/rubrica10/grupo1_examen.pdf', GETDATE()),
(29, 10, 'Examen Abierto - Grupo 2', 'https://evidencias.intec.edu.do/rubrica10/grupo2_examen.pdf', GETDATE()),
(30, 10, 'Examen Abierto - Grupo 3', 'https://evidencias.intec.edu.do/rubrica10/grupo3_examen.pdf', GETDATE());


-- Datos de prueba para historial_incumplimiento
INSERT INTO [dbo].[historial_incumplimiento] 
([Id], [Descripcion], [Fecha], [Id_Usuario]) 
VALUES
-- profesor1
(1, 'No entregó la rúbrica correspondiente al periodo 1-2024.', '2024-04-10', 1),
(2, 'Entregó documentación incompleta del proyecto final.', '2024-05-15', 1),
-- profesor2
(3, 'No participó en la reunión de coordinación docente.', '2024-03-20', 2),
-- profesor3
(4, 'Retraso en la entrega de calificaciones del segundo trimestre.', '2024-06-05', 3),
(5, 'No actualizó el syllabus en el sistema académico.', '2024-01-18', 3),
-- profesor4
(6, 'Faltó a la evaluación de desempeño sin justificación.', '2024-07-12', 4),
-- supervisor1
(7, 'Falta de seguimiento en la revisión de rúbricas entregadas.', '2024-03-25', 7);

/* Los informes sera mejor generarlos desde el API para las pruebas

INSERT INTO [dbo].[informe] 
([Id], [Ruta], [FechaCreacion], [Nombre], [Tipo_Id], [Carrera_Id], [Año], [Trimestre], [Periodo]) 
VALUES
(1, '/informes/informe1.pdf', GETDATE(), 'Informe Anual Ingeniería', 1, 1, 2024, '1', 'Enero-Marzo'),
(2, '/informes/informe2.pdf', GETDATE(), 'Informe Evaluación Software', 2, 2, 2024, '2', 'Abril-Junio');
*/

-- Datos de prueba para profesor_carrera
INSERT INTO [dbo].[profesor_carrera] 
([Profesor_Id], [Carrera_Id]) 
VALUES
-- Juan Pérez (profesor1) -> Ingeniería de Software
(1, 1),
-- Carlos Ruiz (profesor2) -> Ingeniería en Sistemas
(2, 2),
-- Sofía Gómez (profesor3) -> Ambas carreras
(3, 1),
(3, 2),
-- Diego Núñez (profesor4) -> Ingeniería en Sistemas
(4, 2);

-- Datos de prueba para tipo_informe
INSERT INTO [dbo].[tipo_informe] 
([Id], [Descripcion]) 
VALUES
(1, 'Informe de Desempeño');

INSERT INTO [dbo].[usuario] 
([Id], [Username], [Email], [HashedPassword], [Salt], [FechaCreacion], [UltimaEdicion], [FechaEliminacion], [Nombre], [Apellido], [Foto], [CV], [IdSO], [IdEstado], [IdArea], [IdRol], [RolId]) 
VALUES 
(1, 'profesor1', 'profesor1@correo.com', 'hashed_pw', 'salt', GETDATE(), NULL, NULL, 'Juan', 'Pérez', NULL, NULL, 1, 1, 1, 1, 1),
(2, 'profesor2', 'profesor2@correo.com', 'hashed_pw', 'salt', GETDATE(), NULL, NULL, 'Carlos', 'Ruiz', NULL, NULL, 1, 1, 1, 1, 1),
(3, 'profesor3', 'profesor3@correo.com', 'hashed_pw', 'salt', GETDATE(), NULL, NULL, 'Sofía', 'Gómez', NULL, NULL, 1, 1, 1, 1, 1),
(4, 'profesor4', 'profesor4@correo.com', 'hashed_pw', 'salt', GETDATE(), NULL, NULL, 'Diego', 'Núñez', NULL, NULL, 1, 1, 1, 1, 1),
(5, 'admin1', 'admin1@correo.com', 'hashed_pw', 'salt', GETDATE(), NULL, NULL, 'Ana', 'García', NULL, NULL, 1, 1, 1, 2, 2),
(6, 'coordinador1', 'coordinador1@correo.com', 'hashed_pw', 'salt', GETDATE(), NULL, NULL, 'Luis', 'Martínez', NULL, NULL, 1, 1, 1, 3, 3),
(7, 'supervisor1', 'supervisor1@correo.com', 'hashed_pw', 'salt', GETDATE(), NULL, NULL, 'María', 'Fernández', NULL, NULL, 1, 1, 1, 4, 4),
(8, 'auxiliar1', 'auxiliar1@correo.com', 'hashed_pw', 'salt', GETDATE(), NULL, NULL, 'Pedro', 'Ramírez', NULL, NULL, 1, 1, 1, 5, 5),
(9, 'prueba1', 'prueba1@correo.com', 'hashed_pw', 'salt', GETDATE(), NULL, NULL, 'Laura', 'Santos', NULL, NULL, 1, 1, 1, 6, 6),
(10, 'admin2', 'admin2@correo.com', 'hashed_pw', 'salt', GETDATE(), NULL, NULL, 'Marta', 'López', NULL, NULL, 1, 1, 1, 2, 2);


-- Datos de prueba para ConfiguracionEvaluaciones (Falta confirma si esta data esta correcta)
INSERT INTO [dbo].[ConfiguracionEvaluaciones] 
([Id], [Descripcion], [FechaInicio], [FechaCierre], [Id_Estado]) 
VALUES
-- AÑO 2024
(1, 'Evaluación Trimestral 1 - 2024', '2024-01-15', '2024-02-15', 13),  -- Activa
(2, 'Evaluación Trimestral 2 - 2024', '2024-04-15', '2024-05-15', 14),  -- Desactivada
(3, 'Evaluación Trimestral 3 - 2024', '2024-07-15', '2024-08-15', 14),  -- Desactivada
(4, 'Evaluación Trimestral 4 - 2024', '2024-10-15', '2024-11-15', 14),  -- Desactivada
-- AÑO 2025
(5, 'Evaluación Trimestral 1 - 2025', '2025-01-15', '2025-02-15', 13),  -- Activa
(6, 'Evaluación Trimestral 2 - 2025', '2025-04-15', '2025-05-15', 14),  -- Desactivada
(7, 'Evaluación Trimestral 3 - 2025', '2025-07-15', '2025-08-15', 14),  -- Desactivada
(8, 'Evaluación Trimestral 4 - 2025', '2025-10-15', '2025-11-15', 14);  -- Desactivada

-- Datos de prueba para carrera_rubrica 
INSERT INTO [dbo].[carrera_rubrica] 
([Id_Rubrica], [Id_Carrera]) 
VALUES
-- Rubrica 1 (Asignatura 5 → Carrera 1)
(1, 1),
-- Rubrica 2 (Asignatura 8 → Carrera 1)
(2, 1),
-- Rubrica 3 (Asignatura 12 → Carreras 1 y 2)
(3, 1),
(3, 2),
-- Rubrica 4 (Asignatura 15 → Carreras 1 y 2)
(4, 1),
(4, 2),
-- Rubrica 5 (Asignatura 18 → Carrera 1)
(5, 1),
-- Rubrica 6 (Asignatura 20 → Carrera 1)
(6, 1),
-- Rubrica 7 (Asignatura 21 → Carreras 1 y 2)
(7, 1),
(7, 2),
-- Rubrica 8 (Asignatura 23 → Carreras 1 y 2)
(8, 1),
(8, 2),
-- Rubrica 9 (Asignatura 25 → Carrera 1)
(9, 1),
-- Rubrica 10 (Asignatura 26 → Carrera 1)
(10, 1);
