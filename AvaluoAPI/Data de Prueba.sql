INSERT INTO Estados (idtabla, descripcion) VALUES
('Rubrica', 'Activa y sin entregar'),
('Rubrica', 'Activa y entregada'),
('Rubrica', 'Entregada'),
('Rubrica', 'No entregada'),
('Usuario','Activo'),
('Usuario','Desactivado'),
('Asignatura', 'Activa'),
('Asignatura', 'Desactivada'),
('Competencia', 'Activa'),
('Competencia', 'Desactivada')

INSERT INTO metodo_evaluacion ( descripcion) VALUES
('Examen'),
('Tarea'),
('Proyecto'),
('Exposicion')

INSERT INTO Roles (Descripcion) VALUES
('Profesor'),
('Administrador'),
('Coordinador'),
('Supervisor'),
('Auxiliar'),
('Prueba')

INSERT INTO tipo_competencia (Nombre) VALUES
('Específica'),
('General'),
('Negocios')

INSERT INTO [dbo].[areas] 
           ([Id], [Descripcion], [IdCoordinador], [FechaCreacion], [UltimaEdicion])  
     VALUES  
           ( 'Área de Ingeniería', NULL, GETDATE(), NULL),
           ( 'Área de Ciencias Básicas', NULL, GETDATE(), NULL), 
           ( 'Área de Negocios', NULL, GETDATE(), NULL),
           ( 'Área de Idiomas', NULL, GETDATE(), NULL),
           ( 'Área de Humanidades', NULL, GETDATE(), NULL)



INSERT INTO Asignaturas (creditos, codigo, nombre, id_estado, id_area) VALUES 
(4, 'AHC109', 'REDACCION', 7, 5),
(5, 'CBM101', 'ALGEBRA Y GEOMETRIA ANALITICA', 7, 2),
(2, 'CBA106', 'AMBIENTE Y CULTURA', 7, 2),
(2, 'CCP103', 'CREATIVIDAD', 7, 5),
(2, 'IDS207', 'INTRODUCCION A LA INGENIERIA DE SOFTWARE', 7, 1),
(4, 'AHC110', 'ARGUMENTACION LINGUISTICA', 7, 5),
(5, 'CBM102', 'CALCULO DIFERENCIAL', 7, 2),
(4, 'IDS323', 'TECNICAS FUNDAMENTALES DE INGENIERIA DE SOFTWARE', 7, 1),
(2, 'ING102', 'INTRODUCCION A LA PROGRAMACION', 7, 1),
(4, 'CBF210', 'FISICA MECANICA I', 7, 2),
(5, 'CBM201', 'CALCULO INTEGRAL', 7, 2),
(4, 'IDS202', 'TECNOLOGIA DE OBJETOS', 7, 1),
(3, 'IDS340', 'DESARROLLO DE SOFTWARE I', 7, 1),
(5, 'CBM202', 'CALCULO VECTORIAL', 7, 2),
(3, 'IDS341', 'DESARROLLO DE SOFTWARE II', 7, 1),
(4, 'IDS208', 'TEAM BUILDING', 7, 1),
(5, 'CBM208', 'ALGEBRA LINEAL', 7, 2),
(4, 'IDS311', 'PROCESO DE SOFTWARE', 7, 1),
(4, 'IDS324', 'INGENIERIA DE REQUERIMIENTOS DE SOFTWARE', 7, 1),
(3, 'IDS343', 'ESTRUCTURAS DE DATOS Y ALGORITMOS I', 7, 1);



INSERT INTO Competencia (Nombre, Acron, Titulo, Descripcion_ES, Descripcion_EN, id_tipo, id_estado) VALUES
('Student Outcome 1', 'SO1', 'Resolución de problemas complejos de ingeniería', 
 'Capacidad para identificar, formular y resolver problemas complejos de ingeniería aplicando principios de ingeniería, ciencia y matemáticas.', 
 'Ability to identify, formulate, and solve complex engineering problems by applying principles of engineering, science, and mathematics.', 
 1, 9),

('Student Outcome 2', 'SO2', 'Diseño de soluciones de ingeniería', 
 'Capacidad para aplicar el diseño de ingeniería para producir soluciones que satisfagan necesidades específicas, considerando factores de salud pública, seguridad y bienestar, así como factores globales, culturales, sociales, ambientales y económicos.', 
 'Ability to apply engineering design to produce solutions that meet specified needs with consideration of public health, safety, and welfare, as well as global, cultural, social, environmental, and economic factors.', 
 1, 9),

('Student Outcome 3', 'SO3', 'Comunicación efectiva', 
 'Capacidad para comunicarse de manera efectiva con diferentes audiencias.', 
 'Ability to communicate effectively with a range of audiences.', 
 1, 9),

('Student Outcome 4', 'SO4', 'Responsabilidad ética y profesionalismo', 
 'Capacidad para reconocer responsabilidades éticas y profesionales en situaciones de ingeniería y hacer juicios informados, considerando el impacto de las soluciones en contextos globales, económicos, ambientales y sociales.', 
 'Ability to recognize ethical and professional responsibilities in engineering situations and make informed judgments, considering the impact of engineering solutions in global, economic, environmental, and societal contexts.', 
 1, 9),

('Student Outcome 5', 'SO5', 'Trabajo en equipo y liderazgo', 
 'Capacidad para trabajar eficazmente en un equipo cuyos miembros proporcionan liderazgo, crean un entorno colaborativo e inclusivo, establecen objetivos, planifican tareas y cumplen con los objetivos.', 
 'Ability to function effectively on a team whose members provide leadership, create a collaborative and inclusive environment, establish goals, plan tasks, and meet objectives.', 
 1, 9),

('Student Outcome 6', 'SO6', 'Experimentación y análisis de datos', 
 'Capacidad para desarrollar y llevar a cabo experimentos adecuados, analizar e interpretar datos y usar el juicio de ingeniería para sacar conclusiones.', 
 'Ability to develop and conduct appropriate experimentation, analyze and interpret data, and use engineering judgment to draw conclusions.', 
 1, 9),

('Student Outcome 7', 'SO7', 'Aprendizaje continuo y adaptación', 
 'Capacidad para adquirir y aplicar nuevos conocimientos según sea necesario, utilizando estrategias de aprendizaje apropiadas.', 
 'Ability to acquire and apply new knowledge as needed, using appropriate learning strategies.', 
 1, 9);
